// modal.js - Reusable delete confirmation modal
import { api } from './api.js';
import { showToast } from './utils.js';

export function initDeleteModal() {
    const overlay = document.getElementById('deleteModal');
    const cancelBtn = document.getElementById('cancelDelete');
    const confirmBtn = document.getElementById('confirmDelete');
    const orderNumEl = document.getElementById('deleteOrderNumber');
    if (!overlay) return;

    let pendingId = null;

    // Open modal
    document.addEventListener('click', (e) => {
        const btn = e.target.closest('.delete-btn');
        if (!btn) return;
        pendingId = btn.dataset.id;
        if (orderNumEl) orderNumEl.textContent = btn.dataset.number ?? pendingId;
        overlay.hidden = false;
        overlay.classList.add('modal-visible');
        confirmBtn.focus();
    });

    // Close on cancel or backdrop click
    cancelBtn?.addEventListener('click', close);
    overlay.addEventListener('click', (e) => { if (e.target === overlay) close(); });
    document.addEventListener('keydown', (e) => { if (e.key === 'Escape') close(); });

    // Confirm delete
    confirmBtn?.addEventListener('click', async () => {
        if (!pendingId) return;
        confirmBtn.disabled = true;
        confirmBtn.textContent = 'Deleting...';
        try {
            await api.deleteOrder(pendingId);
            showToast('Order deleted successfully.', 'success');
            close();
            // Reload orders table or redirect
            if (window.location.pathname.startsWith('/Orders/')) {
                window.location.href = '/Orders';
            } else {
                // Remove row from table
                const row = document.querySelector(`[data-id="${pendingId}"]`);
                row?.closest('tr, .order-row')?.remove();
                // Trigger refresh if on list page
                window.dispatchEvent(new CustomEvent('orderDeleted', { detail: { id: pendingId } }));
            }
        } catch {
            showToast('Failed to delete order.', 'error');
            close();
        } finally {
            confirmBtn.disabled = false;
            confirmBtn.textContent = 'Delete Order';
        }
    });

    function close() {
        overlay.hidden = true;
        overlay.classList.remove('modal-visible');
        pendingId = null;
    }
}