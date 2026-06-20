// orders-list.js - Orders list page with debounced search, AJAX filtering, pagination
import { debounce, showToast, escHtml, formatCurrency } from './utils.js';

const API_BASE = 'http://localhost:5000/api';

let state = {
    search: '',
    status: '',
    sortBy: 'orderdate',
    sortDesc: true,
    page: 1,
    pageSize: 10,
};

// DOM refs
const searchInput = document.getElementById('searchInput');
const statusFilter = document.getElementById('statusFilter');
const sortBySelect = document.getElementById('sortBy');
const sortDirBtn = document.getElementById('sortDirBtn');
const tableBody = document.getElementById('ordersTableBody');
const paginationContainer = document.getElementById('paginationContainer');
const resultsCount = document.getElementById('resultsCount');
const loadingIndicator = document.getElementById('loadingIndicator');
const searchLoader = document.getElementById('searchLoader');

async function loadOrders() {
    if (loadingIndicator) loadingIndicator.hidden = false;
    if (searchLoader) searchLoader.hidden = false;

    const params = new URLSearchParams({
        page: state.page,
        pageSize: state.pageSize,
        ...(state.search && { search: state.search }),
        ...(state.status && { status: state.status }),
        sortBy: state.sortBy,
        sortDesc: state.sortDesc,
    });

    try {
        const resp = await fetch(`${API_BASE}/orders?${params}`, {
            headers: { 'Accept': 'application/json' }
        });
        if (!resp.ok) throw new Error('Failed to load orders');
        const data = await resp.json();

        renderTable(data.items);
        renderPagination(data);
        if (resultsCount) {
            resultsCount.textContent = `${data.totalCount} order${data.totalCount !== 1 ? 's' : ''} found`;
        }
    } catch (err) {
        showToast('Failed to load orders.', 'error');
    } finally {
        if (loadingIndicator) loadingIndicator.hidden = true;
        if (searchLoader) searchLoader.hidden = true;
    }
}

function renderTable(orders) {
    if (!tableBody) return;

    if (!orders.length) {
        tableBody.innerHTML = `
      <div class="empty-state">
        <svg width="64" height="64" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.5" opacity="0.3">
          <path d="M6 2L3 6v14a2 2 0 002 2h14a2 2 0 002-2V6l-3-4z"/><line x1="3" y1="6" x2="21" y2="6"/>
        </svg>
        <h3>No orders found</h3>
        <p>Try adjusting your filters or <a href="/Orders/Create">create a new order</a>.</p>
      </div>`;
        return;
    }

    const statusClasses = { Pending: 'pending', Processing: 'processing', Shipped: 'shipped', Delivered: 'delivered', Cancelled: 'cancelled' };

    tableBody.innerHTML = `
    <table class="table">
      <thead>
        <tr>
          <th>Order #</th><th>Customer</th><th>Date</th>
          <th class="center">Items</th><th>Total</th><th>Status</th><th>Actions</th>
        </tr>
      </thead>
      <tbody>
        ${orders.map(o => `
          <tr class="order-row" data-id="${escHtml(o.id)}">
            <td><a href="/Orders/${escHtml(o.id)}" class="order-link">${escHtml(o.orderNumber)}</a></td>
            <td>
              <div class="customer-cell">
                <span class="customer-avatar">${escHtml(o.customerName[0])}</span>
                <div>
                  <div class="customer-name">${escHtml(o.customerName)}</div>
                  <div class="customer-email">${escHtml(o.customerEmail)}</div>
                </div>
              </div>
            </td>
            <td class="date-cell">${new Date(o.orderDate).toLocaleDateString('en-US', { month: 'short', day: '2-digit', year: 'numeric' })}</td>
            <td class="center">${o.items.length}</td>
            <td class="amount-cell">${formatCurrency(o.totalAmount)}</td>
            <td><span class="badge badge-${statusClasses[o.statusName] ?? 'pending'}">${escHtml(o.statusName)}</span></td>
            <td>
              <div class="action-buttons">
                <a href="/Orders/${escHtml(o.id)}" class="btn btn-sm btn-ghost" title="View">
                  <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M1 12s4-8 11-8 11 8 11 8-4 8-11 8-11-8-11-8z"/><circle cx="12" cy="12" r="3"/></svg>
                </a>
                <button class="btn btn-sm btn-ghost btn-danger-hover delete-btn"
                  data-id="${escHtml(o.id)}" data-number="${escHtml(o.orderNumber)}" title="Delete">
                  <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><polyline points="3 6 5 6 21 6"/><path d="M19 6v14a2 2 0 01-2 2H7a2 2 0 01-2-2V6m3 0V4a1 1 0 011-1h4a1 1 0 011 1v2"/></svg>
                </button>
              </div>
            </td>
          </tr>`).join('')}
      </tbody>
    </table>`;
}

function renderPagination(data) {
    if (!paginationContainer || data.totalPages <= 1) {
        if (paginationContainer) paginationContainer.innerHTML = '';
        return;
    }

    const { pageNumber: cur, totalPages, totalCount } = data;
    const pages = [];
    for (let i = Math.max(1, cur - 2); i <= Math.min(totalPages, cur + 2); i++) pages.push(i);

    paginationContainer.innerHTML = `
    <div class="pagination">
      ${cur > 1 ? `<button class="btn btn-sm btn-secondary page-btn" data-page="${cur - 1}">← Prev</button>` : ''}
      ${pages.map(p => `<button class="btn btn-sm ${p === cur ? 'btn-primary' : 'btn-secondary'} page-btn" data-page="${p}">${p}</button>`).join('')}
      ${cur < totalPages ? `<button class="btn btn-sm btn-secondary page-btn" data-page="${cur + 1}">Next →</button>` : ''}
      <span class="pagination-info">Page ${cur} of ${totalPages} (${totalCount} total)</span>
    </div>`;

    paginationContainer.querySelectorAll('.page-btn').forEach(btn => {
        btn.addEventListener('click', () => {
            state.page = Number(btn.dataset.page);
            loadOrders();
        });
    });
}

// Initialize
document.addEventListener('DOMContentLoaded', () => {
    // Debounced search
    searchInput?.addEventListener('input', debounce((e) => {
        state.search = e.target.value.trim();
        state.page = 1;
        loadOrders();
    }, 350));

    // Status filter
    statusFilter?.addEventListener('change', () => {
        state.status = statusFilter.value;
        state.page = 1;
        loadOrders();
    });

    // Sort by
    sortBySelect?.addEventListener('change', () => {
        state.sortBy = sortBySelect.value;
        state.page = 1;
        loadOrders();
    });

    // Sort direction toggle
    sortDirBtn?.addEventListener('click', () => {
        state.sortDesc = !state.sortDesc;
        sortDirBtn.dataset.dir = state.sortDesc ? 'desc' : 'asc';
        sortDirBtn.title = state.sortDesc ? 'Descending' : 'Ascending';
        state.page = 1;
        loadOrders();
    });

    // Order deleted event from modal
    window.addEventListener('orderDeleted', () => loadOrders());

    // Initial state from URL params
    const sp = new URLSearchParams(window.location.search);
    if (sp.get('search')) { state.search = sp.get('search'); if (searchInput) searchInput.value = state.search; }
    if (sp.get('status')) { state.status = sp.get('status'); if (statusFilter) statusFilter.value = state.status; }
    if (sp.get('page')) state.page = Number(sp.get('page'));

    loadOrders();
});