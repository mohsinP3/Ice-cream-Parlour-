import express from 'express';
import session from 'express-session';
import path from 'path';
import fs from 'fs';
import multer from 'multer';
import bcrypt from 'bcryptjs';
import { db } from './db.js';

const app = express();
const PORT = 3000;

// Set View Engine
app.set('view engine', 'ejs');
app.set('views', path.join(process.cwd(), 'views'));

// Body Parser & Static Files
app.use(express.urlencoded({ extended: true }));
app.use(express.json());
app.use(express.static(path.join(process.cwd(), 'wwwroot')));

// Ensure uploads folder exists
const uploadsDir = path.join(process.cwd(), 'wwwroot', 'uploads');
if (!fs.existsSync(uploadsDir)) {
  fs.mkdirSync(uploadsDir, { recursive: true });
}

// Multer Storage Configuration
const storage = multer.diskStorage({
  destination: (req, file, cb) => cb(null, uploadsDir),
  filename: (req, file, cb) => {
    const ext = path.extname(file.originalname);
    cb(null, Date.now() + '-' + Math.round(Math.random() * 1e9) + ext);
  }
});
const upload = multer({ storage });

// Session Middleware
app.use(session({
  secret: 'icream-parlour-secret-key-2026',
  resave: false,
  saveUninitialized: true,
  cookie: { maxAge: 24 * 60 * 60 * 1000 }
}));

// Global Context Middleware
app.use((req, res, next) => {
  if (!req.session.cart) req.session.cart = [];

  const userId = req.session.userId;
  let currentUser = null;
  if (userId) {
    currentUser = db.findUserById(userId);
  }

  res.locals.currentUser = currentUser;
  res.locals.cart = req.session.cart;
  res.locals.cartCount = req.session.cart.reduce((acc, item) => acc + item.quantity, 0);

  // Flash messages
  res.locals.successMsg = req.session.successMsg || null;
  res.locals.errorMsg = req.session.errorMsg || null;
  delete req.session.successMsg;
  delete req.session.errorMsg;

  res.locals.activeNav = '';
  next();
});

// Helper Auth Guards
const requireAuth = (req, res, next) => {
  if (!res.locals.currentUser) {
    req.session.errorMsg = 'Please log in to access this page.';
    return res.redirect('/account/login');
  }
  next();
};

const requireAdmin = (req, res, next) => {
  if (!res.locals.currentUser || res.locals.currentUser.role !== 'Admin') {
    req.session.errorMsg = 'Access denied. Admin privileges required.';
    return res.redirect('/account/login');
  }
  next();
};

// ============================================================
// PUBLIC ROUTES
// ============================================================

// Home Index Page
app.get('/', (req, res) => {
  const products = db.getProducts();
  res.render('home/index', {
    title: 'Home - Artisan Ice Cream Parlour',
    activeNav: 'home',
    products
  });
});

// About Page
app.get('/about', (req, res) => {
  res.render('home/about', {
    title: 'About Us',
    activeNav: 'about'
  });
});

// Products Catalog Page
app.get('/products', (req, res) => {
  const categoryId = req.query.category;
  const search = req.query.search ? req.query.search.trim().toLowerCase() : '';

  let products = db.getProducts();
  if (categoryId) {
    products = products.filter(p => p.categoryId == categoryId);
  }
  if (search) {
    products = products.filter(p => p.name.toLowerCase().includes(search) || p.description.toLowerCase().includes(search));
  }

  const categories = db.getCategories();

  res.render('home/product', {
    title: 'Ice Cream Products Catalog',
    activeNav: 'products',
    products,
    categories,
    selectedCategory: categoryId,
    searchQuery: req.query.search || ''
  });
});

// Recipes Guide Page
app.get('/recipes', (req, res) => {
  const recipes = db.getRecipes();
  res.render('home/recipe', {
    title: 'Artisan Ice Cream Recipes',
    activeNav: 'recipes',
    recipes
  });
});

// Recipe Books Page
app.get('/books', (req, res) => {
  const books = db.getBooks();
  res.render('book/index', {
    title: 'Ice Cream Recipe Books',
    activeNav: 'books',
    books
  });
});

// Order Book Page
app.get('/books/order/:id', (req, res) => {
  const book = db.getBookById(req.params.id);
  if (!book) return res.redirect('/books');

  res.render('book/order', {
    title: 'Order Book - ' + book.title,
    activeNav: 'books',
    book
  });
});

app.post('/books/order', (req, res) => {
  const { bookId, customerName, customerEmail, customerPhone, deliveryAddress, notes, totalAmount } = req.body;
  const book = db.getBookById(bookId);

  db.addOrder({
    customerName,
    customerEmail,
    customerPhone,
    deliveryAddress,
    paymentMethod: req.body.paymentMethod || 'Cash on Delivery',
    paymentStatus: 'Paid',
    totalAmount: parseFloat(totalAmount) || (book ? book.price : 20),
    notes: 'Book Order: ' + (book ? book.title : 'Guidebook') + '. ' + (notes || ''),
    items: [
      { productId: 999, productName: book ? book.title : 'Recipe Book', quantity: 1, unitPrice: book ? book.price : 20 }
    ]
  });

  req.session.successMsg = 'Book order placed successfully!';
  res.redirect('/orders/history');
});

// Photo Gallery Page
app.get('/gallery', (req, res) => {
  res.render('home/gallery', {
    title: 'iCREAM Photo Gallery',
    activeNav: 'gallery'
  });
});

// Contact Page
app.get('/contact', (req, res) => {
  res.render('home/contact', {
    title: 'Contact Us',
    activeNav: 'contact'
  });
});

app.post('/contact', (req, res) => {
  req.session.successMsg = 'Thank you for contacting us! We will respond shortly.';
  res.redirect('/contact');
});

// FAQ Page
app.get('/faq', (req, res) => {
  res.render('home/faq', {
    title: 'Frequently Asked Questions',
    activeNav: 'faq'
  });
});

// Feedback Page
app.get('/feedback', (req, res) => {
  const feedbacks = db.getFeedbacks();
  res.render('home/feedback', {
    title: 'Customer Feedback & Reviews',
    activeNav: 'feedback',
    feedbacks
  });
});

app.post('/feedback', (req, res) => {
  const { userName, email, rating, message } = req.body;
  db.addFeedback({
    userId: req.session.userId || null,
    userName,
    email,
    rating,
    message
  });
  req.session.successMsg = 'Thank you for your feedback!';
  res.redirect('/feedback');
});

// Blog Page
app.get('/blog', (req, res) => {
  res.render('home/blog', {
    title: 'Ice Cream Blog & News',
    activeNav: 'blog'
  });
});

// Privacy Page
app.get('/privacy', (req, res) => {
  res.render('home/privacy', {
    title: 'Privacy Policy',
    activeNav: 'privacy'
  });
});

// Newsletter subscription
app.post('/subscribe', (req, res) => {
  req.session.successMsg = 'Thank you for subscribing to iCREAM newsletter!';
  res.redirect('back');
});

// ============================================================
// CART & CHECKOUT ROUTES
// ============================================================

app.get('/cart', (req, res) => {
  const cart = req.session.cart;
  const subtotal = cart.reduce((acc, item) => acc + item.price * item.quantity, 0);

  res.render('cart/index', {
    title: 'Shopping Cart',
    activeNav: '',
    cartItems: cart,
    subtotal
  });
});

app.post('/cart/add', (req, res) => {
  const { productId, quantity } = req.body;
  const qty = parseInt(quantity) || 1;
  const product = db.getProductById(productId);

  if (!product) {
    req.session.errorMsg = 'Product not found.';
    return res.redirect('/products');
  }

  const existingIndex = req.session.cart.findIndex(i => i.productId == productId);
  if (existingIndex > -1) {
    req.session.cart[existingIndex].quantity += qty;
  } else {
    req.session.cart.push({
      productId: product.id,
      name: product.name,
      categoryName: product.categoryName,
      price: product.price,
      imagePath: product.imagePath,
      quantity: qty
    });
  }

  req.session.successMsg = `Added ${qty}x ${product.name} to cart!`;
  res.redirect('/cart');
});

app.post('/cart/update', (req, res) => {
  const { productId, quantity } = req.body;
  const qty = parseInt(quantity) || 1;

  const item = req.session.cart.find(i => i.productId == productId);
  if (item) {
    item.quantity = qty;
  }
  res.redirect('/cart');
});

app.post('/cart/remove', (req, res) => {
  const { productId } = req.body;
  req.session.cart = req.session.cart.filter(i => i.productId != productId);
  res.redirect('/cart');
});

app.get('/checkout', (req, res) => {
  if (req.session.cart.length === 0) {
    req.session.errorMsg = 'Your cart is empty.';
    return res.redirect('/cart');
  }

  const subtotal = req.session.cart.reduce((acc, item) => acc + item.price * item.quantity, 0);

  res.render('cart/checkout', {
    title: 'Checkout & Order Confirmation',
    activeNav: '',
    cartItems: req.session.cart,
    subtotal
  });
});

app.post('/checkout', (req, res) => {
  if (req.session.cart.length === 0) {
    req.session.errorMsg = 'Your cart is empty.';
    return res.redirect('/cart');
  }

  const { customerName, customerEmail, customerPhone, deliveryAddress, paymentMethod, notes } = req.body;
  const subtotal = req.session.cart.reduce((acc, item) => acc + item.price * item.quantity, 0);

  const orderItems = req.session.cart.map(i => ({
    productId: i.productId,
    productName: i.name,
    quantity: i.quantity,
    unitPrice: i.price
  }));

  db.addOrder({
    customerName,
    customerEmail,
    customerPhone,
    deliveryAddress,
    paymentMethod,
    paymentStatus: 'Paid',
    totalAmount: subtotal,
    notes,
    items: orderItems
  });

  req.session.cart = [];
  req.session.successMsg = 'Order placed successfully! Thank you for choosing iCREAM.';
  res.redirect('/orders/history');
});

// Order History
app.get('/orders/history', (req, res) => {
  let orders = [];
  if (res.locals.currentUser) {
    orders = db.getOrdersByEmail(res.locals.currentUser.email);
  } else {
    orders = db.getOrders().slice(0, 5);
  }

  res.render('account/orders', {
    title: 'My Orders & Delivery Tracking',
    activeNav: '',
    orders
  });
});

// ============================================================
// ACCOUNT & AUTH ROUTES
// ============================================================

app.get('/account/login', (req, res) => {
  res.render('account/login', {
    title: 'Login',
    activeNav: ''
  });
});

app.post('/account/login', (req, res) => {
  const { email, password } = req.body;
  const user = db.findUserByEmail(email);

  if (!user || !bcrypt.compareSync(password, user.passwordHash)) {
    req.session.errorMsg = 'Invalid email or password.';
    return res.redirect('/account/login');
  }

  req.session.userId = user.id;
  req.session.successMsg = `Welcome back, ${user.fullName || user.email}!`;

  if (user.role === 'Admin') {
    return res.redirect('/admin/dashboard');
  }
  res.redirect('/account/profile');
});

app.get('/account/register', (req, res) => {
  res.render('account/register', {
    title: 'Create Account',
    activeNav: ''
  });
});

app.post('/account/register', (req, res) => {
  const { fullName, email, password, confirmPassword, address } = req.body;

  if (password !== confirmPassword) {
    req.session.errorMsg = 'Passwords do not match.';
    return res.redirect('/account/register');
  }

  if (db.findUserByEmail(email)) {
    req.session.errorMsg = 'An account with this email already exists.';
    return res.redirect('/account/register');
  }

  const newUser = db.createUser({ fullName, email, password, address });
  req.session.userId = newUser.id;
  req.session.successMsg = 'Account created successfully!';
  res.redirect('/account/profile');
});

app.get('/account/logout', (req, res) => {
  req.session.destroy();
  res.redirect('/');
});

app.get('/account/profile', requireAuth, (req, res) => {
  res.render('account/profile', {
    title: 'My Profile & Dashboard',
    activeNav: ''
  });
});

app.post('/account/profile-update', requireAuth, (req, res) => {
  const { fullName, address } = req.body;
  db.updateUser(req.session.userId, { fullName, address });
  req.session.successMsg = 'Profile details updated.';
  res.redirect('/account/profile');
});

app.post('/account/profile-picture', requireAuth, upload.single('profilePicture'), (req, res) => {
  if (req.file) {
    const profilePicture = '/uploads/' + req.file.filename;
    db.updateUser(req.session.userId, { profilePicture });
    req.session.successMsg = 'Profile picture updated successfully.';
  } else {
    req.session.errorMsg = 'Please select an image file to upload.';
  }
  res.redirect('/account/profile');
});

app.get('/account/forgot-password', (req, res) => {
  res.render('account/forgot_password', {
    title: 'Forgot Password',
    activeNav: ''
  });
});

app.post('/account/forgot-password', (req, res) => {
  req.session.successMsg = 'Password reset link sent to your email address.';
  res.redirect('/account/login');
});

app.get('/account/reset-password', (req, res) => {
  res.render('account/reset_password', {
    title: 'Set New Password',
    activeNav: '',
    token: req.query.token || 'demo-token'
  });
});

app.post('/account/reset-password', (req, res) => {
  const { password, confirmPassword } = req.body;
  if (password !== confirmPassword) {
    req.session.errorMsg = 'Passwords do not match.';
    return res.redirect('back');
  }
  req.session.successMsg = 'Password has been updated successfully! Please login.';
  res.redirect('/account/login');
});

app.post('/account/membership-toggle', requireAuth, (req, res) => {
  const user = db.findUserById(req.session.userId);
  if (user) {
    const newStatus = !user.isPaid;
    db.updateUser(user.id, {
      isPaid: newStatus,
      paymentExpiry: newStatus ? new Date(Date.now() + 30 * 24 * 60 * 60 * 1000).toISOString() : null
    });
    req.session.successMsg = newStatus ? 'VIP Membership activated! You now have full access to exclusive artisan recipes.' : 'VIP Membership cancelled. You are now on the free tier.';
  }
  res.redirect('/account/profile');
});

// ============================================================
// ADMIN PANEL ROUTES
// ============================================================

app.use('/admin', requireAdmin);

// Admin Dashboard
app.get('/admin/dashboard', (req, res) => {
  const orders = db.getOrders();
  const products = db.getProducts();
  const customers = db.getCustomers();

  const totalRevenue = orders.reduce((acc, o) => acc + (o.orderStatus !== 'Cancelled' ? o.totalAmount : 0), 0);

  res.render('admin/dashboard', {
    title: 'Admin Dashboard Overview',
    activeNav: 'dashboard',
    stats: {
      totalOrders: orders.length,
      totalRevenue,
      totalProducts: products.length,
      totalCustomers: customers.length
    },
    recentOrders: orders.slice(0, 5)
  });
});

// Admin Products CRUD
app.get('/admin/products', (req, res) => {
  const products = db.getProducts();
  res.render('admin/products/index', {
    title: 'Product Catalog Management',
    activeNav: 'products',
    products
  });
});

app.get('/admin/products/create', (req, res) => {
  const categories = db.getCategories();
  res.render('admin/products/create', {
    title: 'Add New Product',
    activeNav: 'products',
    categories
  });
});

app.post('/admin/products/create', upload.single('productImage'), (req, res) => {
  const { name, categoryId, price, discountPercent, stockQuantity, lowStockThreshold, description } = req.body;
  const imagePath = req.file ? '/uploads/' + req.file.filename : '/img/product-1.jpg';

  db.addProduct({
    name,
    categoryId,
    price,
    discountPercent,
    stockQuantity,
    lowStockThreshold,
    description,
    imagePath
  });

  req.session.successMsg = 'Product added successfully.';
  res.redirect('/admin/products');
});

app.get('/admin/products/edit/:id', (req, res) => {
  const product = db.getProductById(req.params.id);
  if (!product) return res.redirect('/admin/products');

  const categories = db.getCategories();
  res.render('admin/products/edit', {
    title: 'Edit Product - ' + product.name,
    activeNav: 'products',
    product,
    categories
  });
});

app.post('/admin/products/edit/:id', upload.single('productImage'), (req, res) => {
  const { name, categoryId, price, discountPercent, stockQuantity, lowStockThreshold, description } = req.body;
  const updates = { name, categoryId, price: parseFloat(price), discountPercent: parseFloat(discountPercent), stockQuantity: parseInt(stockQuantity), lowStockThreshold: parseInt(lowStockThreshold), description };

  if (req.file) {
    updates.imagePath = '/uploads/' + req.file.filename;
  }

  db.updateProduct(req.params.id, updates);
  req.session.successMsg = 'Product updated successfully.';
  res.redirect('/admin/products');
});

app.post('/admin/products/delete/:id', (req, res) => {
  db.deleteProduct(req.params.id);
  req.session.successMsg = 'Product deleted successfully.';
  res.redirect('/admin/products');
});

// Admin Categories CRUD
app.get('/admin/categories', (req, res) => {
  const categories = db.getCategories();
  res.render('admin/categories/index', {
    title: 'Category Management',
    activeNav: 'categories',
    categories
  });
});

app.post('/admin/categories/create', (req, res) => {
  const { name, description } = req.body;
  db.addCategory({ name, description });
  req.session.successMsg = 'Category created successfully.';
  res.redirect('/admin/categories');
});

app.post('/admin/categories/delete/:id', (req, res) => {
  db.deleteCategory(req.params.id);
  req.session.successMsg = 'Category deleted.';
  res.redirect('/admin/categories');
});

// Admin Customers Management
app.get('/admin/customers', (req, res) => {
  const customers = db.getCustomers();
  res.render('admin/customers/index', {
    title: 'Customer Accounts Directory',
    activeNav: 'customers',
    customers
  });
});

// Admin Orders Management
app.get('/admin/orders', (req, res) => {
  const orders = db.getOrders();
  res.render('admin/orders/index', {
    title: 'Order Management',
    activeNav: 'orders',
    orders
  });
});

app.get('/admin/orders/details/:id', (req, res) => {
  const order = db.getOrderById(req.params.id);
  if (!order) return res.redirect('/admin/orders');

  res.render('admin/orders/details', {
    title: 'Order Details - ' + order.orderNumber,
    activeNav: 'orders',
    order
  });
});

app.post('/admin/orders/update-status/:id', (req, res) => {
  const { orderStatus } = req.body;
  db.updateOrderStatus(req.params.id, orderStatus, orderStatus === 'Delivered' ? 'Paid' : null);
  req.session.successMsg = 'Order status updated to ' + orderStatus + '.';
  res.redirect('/admin/orders');
});

// Admin Books Management
app.get('/admin/books', (req, res) => {
  const books = db.getBooks();
  res.render('admin/books/index', {
    title: 'Recipe Books Catalog Management',
    activeNav: 'books',
    books
  });
});

app.post('/admin/books/create', (req, res) => {
  const { title, author, price, description } = req.body;
  db.addBook({ title, author, price, description });
  req.session.successMsg = 'Book added to catalog.';
  res.redirect('/admin/books');
});

app.post('/admin/books/delete/:id', (req, res) => {
  db.deleteBook(req.params.id);
  req.session.successMsg = 'Book deleted.';
  res.redirect('/admin/books');
});

// Admin Recipes Management
app.get('/admin/recipes', (req, res) => {
  const recipes = db.getRecipes();
  res.render('admin/recipes/index', {
    title: 'Recipes Directory Management',
    activeNav: 'recipes',
    recipes
  });
});

app.post('/admin/recipes/create', (req, res) => {
  const { name, category, isFree, ingredients, procedure } = req.body;
  db.addRecipe({ name, category, isFree: isFree === 'true', ingredients, procedure });
  req.session.successMsg = 'Recipe added successfully.';
  res.redirect('/admin/recipes');
});

app.post('/admin/recipes/delete/:id', (req, res) => {
  db.deleteRecipe(req.params.id);
  req.session.successMsg = 'Recipe deleted.';
  res.redirect('/admin/recipes');
});

// Admin Reports Management
app.get('/admin/reports', (req, res) => {
  const orders = db.getOrders();
  const categories = db.getCategories();
  const products = db.getProducts();

  const totalRevenue = orders.reduce((acc, o) => acc + (o.orderStatus !== 'Cancelled' ? o.totalAmount : 0), 0);
  const completedOrders = orders.filter(o => o.orderStatus === 'Delivered').length;

  res.render('admin/reports/index', {
    title: 'Sales & Analytics Reporting',
    activeNav: 'reports',
    totalRevenue,
    completedOrders,
    categories,
    products
  });
});

// Fallback 404
app.use((req, res) => {
  res.status(404).render('home/index', {
    title: 'Home - iCREAM Parlour',
    activeNav: 'home',
    products: db.getProducts()
  });
});

// Start Server
app.listen(PORT, '0.0.0.0', () => {
  console.log(`iCREAM Parlour Server running on http://0.0.0.0:${PORT}`);
});
