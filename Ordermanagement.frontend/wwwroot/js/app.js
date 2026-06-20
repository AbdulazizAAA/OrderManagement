// app.js - Main entry point, bootstraps shared modules
import { initDeleteModal } from './modal.js';

document.addEventListener('DOMContentLoaded', () => {
    initDeleteModal();

    // Auto-dismiss alerts
    const alert = document.getElementById('globalAlert');
    if (alert) setTimeout(() => alert.remove(), 5000);
});