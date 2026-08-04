import os
import sys
from playwright.sync_api import sync_playwright

def run_cuj(page):
    # Navigate to login page
    print("Navigating to Login Page...")
    page.goto("http://localhost:5274/Account/Login")
    page.wait_for_timeout(1000)

    # Fill credentials
    print("Filling Credentials...")
    page.fill("input[name='Email']", "admin@icream.com")
    page.wait_for_timeout(500)
    page.fill("input[name='Password']", "Admin@123")
    page.wait_for_timeout(500)

    # Click Sign In
    print("Clicking Sign In...")
    page.click("button[type='submit']")
    page.wait_for_timeout(4000)

    # Take screenshot of Admin Dashboard
    print("Capturing Admin Dashboard Screenshot...")
    os.makedirs("verification/screenshots", exist_ok=True)
    page.screenshot(path="verification/screenshots/verification.png")
    page.wait_for_timeout(1000)
    print("Frontend Verification CUJ completed successfully!")

if __name__ == "__main__":
    with sync_playwright() as p:
        browser = p.chromium.launch(headless=True)
        context = browser.new_context(
            record_video_dir="verification/videos",
            ignore_https_errors=True
        )
        page = context.new_page()
        try:
            run_cuj(page)
        except Exception as e:
            print(f"Error occurred: {e}")
        finally:
            context.close()
            browser.close()
