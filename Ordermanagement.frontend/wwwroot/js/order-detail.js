// order-detail.js - Order detail page
import { api } from './api.js';
import { showToast } from './utils.js';

document.addEventListener('DOMContentLoaded', () => {
    const updateBtn = document.getElementById('updateStatusBtn');
    const statusSelect = document.getElementById('statusSelect');
    const statusMsg = document.getElementById('statusMessage');

    updateBtn?.addEventListener('click', async () => {
        const orderId = updateBtn.dataset.id;
        const newStatus = parseInt(statusSelect.value);
        updateBtn.disabled = true;
        updateBtn.textContent = 'Updating...';

        try {
            const updated = await api.updateStatus(orderId, newStatus);
            showToast('Status updated successfully!', 'success');

            // AJAX partial update — update the badge in the header
            const badge = document.querySelector('.page-header .badge');
            if (badge) {
                const statusMap = { Pending: 'pending', Processing: 'processing', Shipped: 'shipped', Delivered: 'delivered', Cancelled: 'cancelled' };
                badge.className = `badge badge-${statusMap[updated.statusName] ?? 'pending'}`;
                badge.textContent = updated.statusName;
            }

            if (statusMsg) {
                statusMsg.hidden = false;
                statusMsg.className = 'status-message status-success';
                statusMsg.textContent = `✓ Status changed to ${updated.statusName}`;
                setTimeout(() => { statusMsg.hidden = true; }, 3000);
            }
        } catch {
            showToast('Failed to update status.', 'error');
        } finally {
            updateBtn.disabled = false;
            updateBtn.textContent = 'Update Status';
        }
    });
});