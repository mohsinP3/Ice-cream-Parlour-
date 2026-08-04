import fs from 'fs';
import path from 'path';
import bcrypt from 'bcryptjs';

const DATA_FILE = path.join(process.cwd(), 'data_store.json');

// Default Seed Data
const defaultData = {
  users: [
    {
      id: 'admin-1',
      email: 'admin@icream.com',
      passwordHash: bcrypt.hashSync('admin123', 10),
      fullName: 'Admin Manager',
      role: 'Admin',
      address: '123 Parlour Street, New York, NY',
      isPaid: true,
      paymentExpiry: new Date(Date.now() + 365 * 24 * 60 * 60 * 1000).toISOString(),
      createdAt: new Date().toISOString(),
      profilePicture: '/img/team-1.jpg'
    },
    {
      id: 'user-1',
      email: 'user@icream.com',
      passwordHash: bcrypt.hashSync('user123', 10),
      fullName: 'John Anderson',
      role: 'User',
      address: '456 Sweet Ave, New York, NY',
      isPaid: true,
      paymentExpiry: new Date(Date.now() + 30 * 24 * 60 * 60 * 1000).toISOString(),
      createdAt: new Date().toISOString(),
      profilePicture: '/img/team-2.jpg'
    },
    {
      id: 'user-2',
      email: 'maria@gmail.com',
      passwordHash: bcrypt.hashSync('maria123', 10),
      fullName: 'Maria Garcia',
      role: 'User',
      address: '789 Cream Way, Brooklyn, NY',
      isPaid: false,
      paymentExpiry: null,
      createdAt: new Date().toISOString(),
      profilePicture: '/img/team-3.jpg'
    }
  ],
  categories: [
    { id: 1, name: 'Artisan Scoops', description: 'Handcrafted creamy ice cream scoops', isActive: true, createdDate: '2026-01-01' },
    { id: 2, name: 'Sundaes & Parfaits', description: 'Rich ice cream sundaes layered with toppings', isActive: true, createdDate: '2026-01-01' },
    { id: 3, name: 'Gelato & Sorbet', description: 'Authentic Italian style gelatos and refreshing sorbets', isActive: true, createdDate: '2026-01-01' },
    { id: 4, name: 'Ice Cream Cakes', description: 'Celebration ice cream cakes for special events', isActive: true, createdDate: '2026-01-01' },
    { id: 5, name: 'Milkshakes & Smoothies', description: 'Thick, creamy milkshakes and fruit blends', isActive: true, createdDate: '2026-01-01' }
  ],
  products: [
    {
      id: 1,
      name: 'Madagascar Vanilla Bean',
      description: 'Classic rich vanilla made with real Madagascar vanilla pods and organic cream.',
      categoryId: 1,
      categoryName: 'Artisan Scoops',
      price: 8.50,
      discountPercent: 10,
      stockQuantity: 45,
      lowStockThreshold: 10,
      barcode: '123456789012',
      productCode: 'PROD-VAN-01',
      imagePath: '/img/product-1.jpg',
      status: 'Active',
      createdAt: '2026-01-10'
    },
    {
      id: 2,
      name: 'Belgian Dark Chocolate Fudge',
      description: 'Deep 70% dark Belgian chocolate scoop swirled with decadent hot fudge.',
      categoryId: 1,
      categoryName: 'Artisan Scoops',
      price: 9.99,
      discountPercent: 15,
      stockQuantity: 30,
      lowStockThreshold: 10,
      barcode: '123456789013',
      productCode: 'PROD-CHO-02',
      imagePath: '/img/product-2.jpg',
      status: 'Active',
      createdAt: '2026-01-12'
    },
    {
      id: 3,
      name: 'Fresh Strawberry Shortcake Gelato',
      description: 'Made with organic ripe strawberries and graham cracker biscuit crumbles.',
      categoryId: 3,
      categoryName: 'Gelato & Sorbet',
      price: 10.50,
      discountPercent: 0,
      stockQuantity: 22,
      lowStockThreshold: 5,
      barcode: '123456789014',
      productCode: 'PROD-STR-03',
      imagePath: '/img/product-3.jpg',
      status: 'Active',
      createdAt: '2026-01-15'
    },
    {
      id: 4,
      name: 'Mango Passion Fruit Sorbet',
      description: '100% dairy-free tropical mango and tangy passionfruit sorbet.',
      categoryId: 3,
      categoryName: 'Gelato & Sorbet',
      price: 7.99,
      discountPercent: 5,
      stockQuantity: 50,
      lowStockThreshold: 10,
      barcode: '123456789015',
      productCode: 'PROD-MAN-04',
      imagePath: '/img/product-4.jpg',
      status: 'Active',
      createdAt: '2026-01-18'
    },
    {
      id: 5,
      name: 'Sicilian Pistachio Crunch',
      description: 'Roasted Bronte pistachios blended into silky smooth Italian gelato.',
      categoryId: 3,
      categoryName: 'Gelato & Sorbet',
      price: 11.25,
      discountPercent: 0,
      stockQuantity: 18,
      lowStockThreshold: 5,
      barcode: '123456789016',
      productCode: 'PROD-PIS-05',
      imagePath: '/img/product-5.jpg',
      status: 'Active',
      createdAt: '2026-01-20'
    },
    {
      id: 6,
      name: 'Salted Caramel Butterscotch',
      description: 'Caramelized brown sugar with sea salt chunks and butterscotch crunch.',
      categoryId: 2,
      categoryName: 'Sundaes & Parfaits',
      price: 12.00,
      discountPercent: 20,
      stockQuantity: 8,
      lowStockThreshold: 10,
      barcode: '123456789017',
      productCode: 'PROD-CAR-06',
      imagePath: '/img/product-6.jpg',
      status: 'Active',
      createdAt: '2026-01-22'
    },
    {
      id: 7,
      name: 'Triple Berry Celebration Cake',
      description: 'Multi-layer vanilla and berry ice cream cake with whipped cream frosting.',
      categoryId: 4,
      categoryName: 'Ice Cream Cakes',
      price: 45.00,
      discountPercent: 10,
      stockQuantity: 12,
      lowStockThreshold: 3,
      barcode: '123456789018',
      productCode: 'PROD-CAK-07',
      imagePath: '/img/product-7.jpg',
      status: 'Active',
      createdAt: '2026-01-25'
    },
    {
      id: 8,
      name: 'Ultimate Cookies & Cream Shake',
      description: 'Thick creamy milkshake blended with Oreo cookies and chocolate drizzle.',
      categoryId: 5,
      categoryName: 'Milkshakes & Smoothies',
      price: 8.99,
      discountPercent: 0,
      stockQuantity: 60,
      lowStockThreshold: 15,
      barcode: '123456789019',
      productCode: 'PROD-MIL-08',
      imagePath: '/img/product-8.jpg',
      status: 'Active',
      createdAt: '2026-01-28'
    }
  ],
  recipes: [
    {
      id: 1,
      name: 'Classic Madagascar Vanilla Bean',
      category: 'Artisan Scoops',
      ingredients: '2 cups heavy cream, 1 cup whole milk, 3/4 cup sugar, 1 vanilla bean split, pinch of salt',
      procedure: '1. Heat cream, milk, sugar, and scraped vanilla bean pod in sauce pan until warm.\n2. Chill in refrigerator for 4 hours.\n3. Churn in ice cream maker for 25 mins.\n4. Freeze for 2 hours before serving.',
      imagePath: '/img/recipe-1.jpg',
      isFree: true,
      price: 0,
      createdDate: '2026-01-10'
    },
    {
      id: 2,
      name: 'Triple Chocolate Fudge Supreme',
      category: 'Artisan Scoops',
      ingredients: '2 cups heavy cream, 1 cup dark milk, 1/2 cup Dutch cocoa powder, 6oz 70% dark chocolate chunks, 3/4 cup sugar',
      procedure: '1. Whisk cocoa powder and sugar into warm milk until dissolved.\n2. Melt dark chocolate into cream mixture.\n3. Chill thoroughly and churn.\n4. Fold in chocolate fudge ribbons.',
      imagePath: '/img/recipe-2.jpg',
      isFree: false,
      price: 15.00,
      createdDate: '2026-01-12'
    },
    {
      id: 3,
      name: 'Fresh Mango & Passionfruit Sorbet',
      category: 'Gelato & Sorbet',
      ingredients: '3 cups fresh mango puree, 1/2 cup passionfruit juice, 3/4 cup simple syrup, 1 tbsp lemon juice',
      procedure: '1. Blend fresh ripe mangoes with passionfruit juice and simple syrup.\n2. Strain through fine mesh strainer.\n3. Chill for 2 hours and churn until smooth and icy.\n4. Garnish with mint leaves.',
      imagePath: '/img/recipe-3.jpg',
      isFree: true,
      price: 0,
      createdDate: '2026-01-15'
    },
    {
      id: 4,
      name: 'Sicilian Roasted Pistachio Gelato',
      category: 'Gelato & Sorbet',
      ingredients: '1 cup roasted unsalted pistachios ground into paste, 2 cups milk, 1 cup heavy cream, 3/4 cup sugar, egg yolks',
      procedure: '1. Make custard base with milk, sugar, and egg yolks heated to 175°F.\n2. Whisk in rich pistachio paste.\n3. Chill 6 hours, then process in gelato machine for ultra-dense creaminess.',
      imagePath: '/img/recipe-4.jpg',
      isFree: false,
      price: 15.00,
      createdDate: '2026-01-18'
    }
  ],
  books: [
    {
      id: 1,
      title: 'Artisan Ice Cream Making at Home',
      author: 'Chef Antonio Rossi',
      description: 'Master 50+ traditional Italian gelatos, sorbets, and frozen desserts using simple home equipment.',
      price: 24.99,
      imagePath: '/img/book-1.jpg',
      stockQuantity: 25
    },
    {
      id: 2,
      title: 'The Ultimate Sundae & Toppings Cookbook',
      author: 'Sarah Jenkins',
      description: 'Learn to create hot fudges, caramel sauces, candied nuts, and waffle cones from scratch.',
      price: 19.99,
      imagePath: '/img/book-2.jpg',
      stockQuantity: 40
    },
    {
      id: 3,
      title: 'Dairy-Free & Vegan Frozen Treats',
      author: 'Maya Lin',
      description: 'Delicious plant-based sorbets, coconut-cream scoops, and nut-milk frozen delights.',
      price: 22.50,
      imagePath: '/img/book-3.jpg',
      stockQuantity: 15
    }
  ],
  customers: [
    {
      id: 1,
      fullName: 'John Anderson',
      email: 'user@icream.com',
      phone: '+1 (555) 234-5678',
      address: '456 Sweet Ave, New York, NY 10001',
      profileImagePath: '/img/team-2.jpg',
      isActive: true,
      createdAt: '2026-01-05',
      lastActivityDate: '2026-08-01'
    },
    {
      id: 2,
      fullName: 'Maria Garcia',
      email: 'maria@gmail.com',
      phone: '+1 (555) 987-6543',
      address: '789 Cream Way, Brooklyn, NY 11201',
      profileImagePath: '/img/team-3.jpg',
      isActive: true,
      createdAt: '2026-01-12',
      lastActivityDate: '2026-07-28'
    },
    {
      id: 3,
      fullName: 'Robert Wilson',
      email: 'robert.w@gmail.com',
      phone: '+1 (555) 345-6789',
      address: '101 Ice Street, Manhattan, NY 10002',
      profileImagePath: '/img/team-1.jpg',
      isActive: true,
      createdAt: '2026-02-01',
      lastActivityDate: '2026-08-03'
    }
  ],
  orders: [
    {
      id: 1,
      orderNumber: 'ORD-20260801-1042',
      orderDate: '2026-08-01T14:30:00.000Z',
      customerId: 1,
      customerName: 'John Anderson',
      customerEmail: 'user@icream.com',
      customerPhone: '+1 (555) 234-5678',
      deliveryAddress: '456 Sweet Ave, New York, NY 10001',
      totalAmount: 28.49,
      orderStatus: 'Delivered',
      paymentStatus: 'Paid',
      paymentMethod: 'Credit Card',
      notes: 'Please drop off at reception desk',
      items: [
        { id: 101, productId: 1, productName: 'Madagascar Vanilla Bean', quantity: 2, unitPrice: 8.50 },
        { id: 102, productId: 5, productName: 'Sicilian Pistachio Crunch', quantity: 1, unitPrice: 11.49 }
      ]
    },
    {
      id: 2,
      orderNumber: 'ORD-20260803-2198',
      orderDate: '2026-08-03T10:15:00.000Z',
      customerId: 2,
      customerName: 'Maria Garcia',
      customerEmail: 'maria@gmail.com',
      customerPhone: '+1 (555) 987-6543',
      deliveryAddress: '789 Cream Way, Brooklyn, NY 11201',
      totalAmount: 45.00,
      orderStatus: 'Processing',
      paymentStatus: 'Paid',
      paymentMethod: 'PayPal',
      notes: 'Include birthday candle package',
      items: [
        { id: 103, productId: 7, productName: 'Triple Berry Celebration Cake', quantity: 1, unitPrice: 45.00 }
      ]
    }
  ],
  feedbacks: [
    {
      id: 1,
      userId: 'user-1',
      userName: 'John Anderson',
      email: 'user@icream.com',
      message: 'The Madagascar Vanilla Bean and Pistachio gelatos are absolutely mind blowing! Highly recommend joining membership.',
      rating: 5,
      submittedDate: '2026-08-01',
      isRegistered: true,
      isRead: true
    },
    {
      id: 2,
      userId: 'user-2',
      userName: 'Maria Garcia',
      email: 'maria@gmail.com',
      message: 'Loved the fast delivery and packaging. The ice cream arrived completely frozen even in August heat!',
      rating: 5,
      submittedDate: '2026-08-02',
      isRegistered: true,
      isRead: false
    }
  ]
};

class Database {
  constructor() {
    this.load();
  }

  load() {
    try {
      if (fs.existsSync(DATA_FILE)) {
        const content = fs.readFileSync(DATA_FILE, 'utf-8');
        this.data = JSON.parse(content);
      } else {
        this.data = JSON.parse(JSON.stringify(defaultData));
        this.save();
      }
    } catch (err) {
      console.error('Error loading data file:', err);
      this.data = JSON.parse(JSON.stringify(defaultData));
    }

    // Ensure users array exists
    if (!this.data.users) this.data.users = [];

    // Ensure Seed Admin User exists
    let admin = this.data.users.find(u => u.email.toLowerCase() === 'admin@icream.com');
    if (!admin) {
      admin = {
        id: 'admin-1',
        email: 'admin@icream.com',
        passwordHash: bcrypt.hashSync('admin123', 10),
        fullName: 'Admin Manager',
        role: 'Admin',
        address: '123 Parlour Street, New York, NY',
        isPaid: true,
        paymentExpiry: new Date(Date.now() + 365 * 24 * 60 * 60 * 1000).toISOString(),
        createdAt: new Date().toISOString(),
        profilePicture: '/img/team-1.jpg'
      };
      this.data.users.push(admin);
      this.save();
    } else {
      // Ensure admin role & password match default
      admin.role = 'Admin';
      admin.passwordHash = bcrypt.hashSync('admin123', 10);
    }

    // Ensure Seed User exists
    let user = this.data.users.find(u => u.email.toLowerCase() === 'user@icream.com');
    if (!user) {
      this.data.users.push({
        id: 'user-1',
        email: 'user@icream.com',
        passwordHash: bcrypt.hashSync('user123', 10),
        fullName: 'John Anderson',
        role: 'User',
        address: '456 Sweet Ave, New York, NY',
        isPaid: true,
        paymentExpiry: new Date(Date.now() + 30 * 24 * 60 * 60 * 1000).toISOString(),
        createdAt: new Date().toISOString(),
        profilePicture: '/img/team-2.jpg'
      });
      this.save();
    }

    // Ensure array properties exist and have sample data
    ['categories', 'products', 'recipes', 'books', 'customers', 'orders', 'feedbacks'].forEach(key => {
      if (!this.data[key] || !Array.isArray(this.data[key]) || this.data[key].length === 0) {
        this.data[key] = JSON.parse(JSON.stringify(defaultData[key] || []));
        this.save();
      }
    });
  }

  save() {
    try {
      fs.writeFileSync(DATA_FILE, JSON.stringify(this.data, null, 2), 'utf-8');
    } catch (err) {
      console.error('Error saving data file:', err);
    }
  }

  // ===== USERS =====
  getUsers() { return this.data.users; }
  findUserByEmail(email) {
    return this.data.users.find(u => u.email.toLowerCase() === email.toLowerCase());
  }
  findUserById(id) {
    return this.data.users.find(u => u.id === id);
  }
  createUser(userData) {
    const newUser = {
      id: 'user-' + Date.now(),
      email: userData.email,
      passwordHash: bcrypt.hashSync(userData.password, 10),
      fullName: userData.fullName,
      role: 'User',
      address: userData.address || '',
      isPaid: false,
      paymentExpiry: null,
      createdAt: new Date().toISOString(),
      profilePicture: ''
    };
    this.data.users.push(newUser);
    this.save();
    return newUser;
  }
  updateUser(id, updates) {
    const user = this.findUserById(id);
    if (user) {
      Object.assign(user, updates);
      this.save();
    }
    return user;
  }

  // ===== CATEGORIES =====
  getCategories() { return this.data.categories; }
  getCategoryById(id) { return this.data.categories.find(c => c.id == id); }
  addCategory(category) {
    const newCat = {
      id: Date.now(),
      name: category.name,
      description: category.description || '',
      isActive: category.isActive !== undefined ? Boolean(category.isActive) : true,
      createdDate: new Date().toISOString().split('T')[0]
    };
    this.data.categories.push(newCat);
    this.save();
    return newCat;
  }
  updateCategory(id, updates) {
    const cat = this.getCategoryById(id);
    if (cat) {
      Object.assign(cat, updates);
      this.save();
    }
    return cat;
  }
  deleteCategory(id) {
    this.data.categories = this.data.categories.filter(c => c.id != id);
    this.save();
  }

  // ===== PRODUCTS =====
  getProducts() { return this.data.products; }
  getProductById(id) { return this.data.products.find(p => p.id == id); }
  addProduct(productData) {
    const cat = this.getCategoryById(productData.categoryId);
    const newProd = {
      id: Date.now(),
      name: productData.name,
      description: productData.description || '',
      categoryId: parseInt(productData.categoryId),
      categoryName: cat ? cat.name : 'General',
      price: parseFloat(productData.price) || 0,
      discountPercent: parseFloat(productData.discountPercent) || 0,
      stockQuantity: parseInt(productData.stockQuantity) || 0,
      lowStockThreshold: parseInt(productData.lowStockThreshold) || 10,
      barcode: productData.barcode || Math.floor(100000000000 + Math.random() * 900000000000).toString(),
      productCode: 'PROD-' + Date.now().toString().slice(-4),
      imagePath: productData.imagePath || '/img/product-1.jpg',
      status: productData.status || 'Active',
      createdAt: new Date().toISOString().split('T')[0]
    };
    this.data.products.push(newProd);
    this.save();
    return newProd;
  }
  updateProduct(id, updates) {
    const prod = this.getProductById(id);
    if (prod) {
      if (updates.categoryId) {
        const cat = this.getCategoryById(updates.categoryId);
        updates.categoryName = cat ? cat.name : prod.categoryName;
      }
      Object.assign(prod, updates);
      this.save();
    }
    return prod;
  }
  deleteProduct(id) {
    this.data.products = this.data.products.filter(p => p.id != id);
    this.save();
  }

  // ===== RECIPES =====
  getRecipes() { return this.data.recipes; }
  getRecipeById(id) { return this.data.recipes.find(r => r.id == id); }
  addRecipe(recipe) {
    const newRecipe = {
      id: Date.now(),
      name: recipe.name,
      category: recipe.category,
      ingredients: recipe.ingredients,
      procedure: recipe.procedure,
      imagePath: recipe.imagePath || '/img/recipe-1.jpg',
      isFree: recipe.isFree === true || recipe.isFree === 'true',
      price: parseFloat(recipe.price) || 0,
      createdDate: new Date().toISOString().split('T')[0]
    };
    this.data.recipes.push(newRecipe);
    this.save();
    return newRecipe;
  }
  updateRecipe(id, updates) {
    const r = this.getRecipeById(id);
    if (r) {
      Object.assign(r, updates);
      this.save();
    }
    return r;
  }
  deleteRecipe(id) {
    this.data.recipes = this.data.recipes.filter(r => r.id != id);
    this.save();
  }

  // ===== BOOKS =====
  getBooks() { return this.data.books; }
  getBookById(id) { return this.data.books.find(b => b.id == id); }
  addBook(book) {
    const newBook = {
      id: Date.now(),
      title: book.title,
      author: book.author,
      description: book.description,
      price: parseFloat(book.price) || 0,
      imagePath: book.imagePath || '/img/book-1.jpg',
      stockQuantity: parseInt(book.stockQuantity) || 10
    };
    this.data.books.push(newBook);
    this.save();
    return newBook;
  }
  updateBook(id, updates) {
    const b = this.getBookById(id);
    if (b) {
      Object.assign(b, updates);
      this.save();
    }
    return b;
  }
  deleteBook(id) {
    this.data.books = this.data.books.filter(b => b.id != id);
    this.save();
  }

  // ===== CUSTOMERS =====
  getCustomers() { return this.data.customers; }
  getCustomerById(id) { return this.data.customers.find(c => c.id == id); }
  getCustomerByEmail(email) { return this.data.customers.find(c => c.email.toLowerCase() === email.toLowerCase()); }
  addCustomer(customer) {
    const newC = {
      id: Date.now(),
      fullName: customer.fullName,
      email: customer.email,
      phone: customer.phone || '',
      address: customer.address || '',
      profileImagePath: customer.profileImagePath || '/img/team-1.jpg',
      isActive: true,
      createdAt: new Date().toISOString().split('T')[0],
      lastActivityDate: new Date().toISOString().split('T')[0]
    };
    this.data.customers.push(newC);
    this.save();
    return newC;
  }

  // ===== ORDERS =====
  getOrders() { return this.data.orders; }
  getOrderById(id) { return this.data.orders.find(o => o.id == id); }
  getOrdersByEmail(email) { return this.data.orders.filter(o => o.customerEmail.toLowerCase() === email.toLowerCase()); }
  addOrder(orderData) {
    let customer = this.getCustomerByEmail(orderData.customerEmail);
    if (!customer) {
      customer = this.addCustomer({
        fullName: orderData.customerName,
        email: orderData.customerEmail,
        phone: orderData.customerPhone,
        address: orderData.deliveryAddress
      });
    }

    const newOrder = {
      id: Date.now(),
      orderNumber: 'ORD-' + new Date().toISOString().slice(0,10).replace(/-/g,'') + '-' + Math.floor(1000 + Math.random() * 9000),
      orderDate: new Date().toISOString(),
      customerId: customer.id,
      customerName: orderData.customerName,
      customerEmail: orderData.customerEmail,
      customerPhone: orderData.customerPhone || '',
      deliveryAddress: orderData.deliveryAddress || '',
      totalAmount: parseFloat(orderData.totalAmount) || 0,
      orderStatus: 'Pending',
      paymentStatus: orderData.paymentStatus || 'Pending',
      paymentMethod: orderData.paymentMethod || 'Cash on Delivery',
      notes: orderData.notes || '',
      items: orderData.items || []
    };

    // Deduct product inventory
    if (newOrder.items && newOrder.items.length > 0) {
      newOrder.items.forEach(item => {
        const prod = this.getProductById(item.productId);
        if (prod && prod.stockQuantity >= item.quantity) {
          prod.stockQuantity -= item.quantity;
        }
      });
    }

    this.data.orders.unshift(newOrder);
    this.save();
    return newOrder;
  }
  updateOrderStatus(id, orderStatus, paymentStatus) {
    const order = this.getOrderById(id);
    if (order) {
      if (orderStatus) order.orderStatus = orderStatus;
      if (paymentStatus) order.paymentStatus = paymentStatus;
      this.save();
    }
    return order;
  }

  // ===== FEEDBACKS =====
  getFeedbacks() { return this.data.feedbacks; }
  addFeedback(feedback) {
    const newF = {
      id: Date.now(),
      userId: feedback.userId || null,
      userName: feedback.userName,
      email: feedback.email,
      message: feedback.message,
      rating: parseInt(feedback.rating) || 5,
      submittedDate: new Date().toISOString().split('T')[0],
      isRegistered: Boolean(feedback.userId),
      isRead: false
    };
    this.data.feedbacks.unshift(newF);
    this.save();
    return newF;
  }
}

export const db = new Database();
