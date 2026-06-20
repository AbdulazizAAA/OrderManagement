// utils.js - Shared utility functions

// Debounce function for search input
export function debounce(fn, delay = 350) {
    let timer;
    return (...args) => {
        clearTimeout(timer);
        timer = setTimeout(() => fn(...args), delay);
    };
}

// Format currency
export function formatCurrency(amount) {
    return new Intl.NumberFormat('en-US', { style: 'currency', currency: 'USD' }).format(amount);
}

// Show toast notification
export function showToast(message, type = 'success') {
    const toast = document.getElementById('toast');
    if (!toast) return;
    toast.textContent = message;
    toast.className = `toast toast-${type}`;
    toast.hidden = false;
    setTimeout(() => { toast.hidden = true; }, 3500);
}

// Show/hide loading state on element
export function setLoading(el, loading) {
    if (!el) return;
    el.disabled = loading;
    el.dataset.loading = loading;
}

// Validate a single input field
export function validateField(input) {
    const error = input.nextElementSibling;
    if (!input.checkValidity()) {
        input.classList.add('input-error');
        if (error?.classList.contains('field-error')) error.textContent = input.validationMessage;
        return false;
    }
    input.classList.remove('input-error');
    if (error?.classList.contains('field-error')) error.textContent = '';
    return true;
}

// Escape HTML to prevent XSS
export function escHtml(str) {
    return String(str).replace(/[&<>"']/g, c =>
        ({ '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;' }[c]));
}