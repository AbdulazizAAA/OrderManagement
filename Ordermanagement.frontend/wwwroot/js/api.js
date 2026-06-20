// api.js - Centralized API client module
const API_BASE = 'https://localhost:5555/api';

async function request(method, path, body = null) {
    const opts = {
        method,
        headers: { 'Content-Type': 'application/json', 'Accept': 'application/json' },
    };
    if (body) opts.body = JSON.stringify(body);

    const resp = await fetch(`${API_BASE}${path}`, opts);
    if (!resp.ok) {
        const err = await resp.json().catch(() => ({ message: 'Request failed' }));
        throw Object.assign(new Error(err.message ?? 'API Error'), { status: resp.status, body: err });
    }
    if (resp.status === 204) return null;
    return resp.json();
}
const api = {
    // Orders
    getOrders: (params) => request('GET', `/orders?${new URLSearchParams(params)}`),
    getOrder: (id) => request('GET', `/orders/${id}`),
    createOrder: (data) => request('POST', '/orders', data),
    updateStatus: (id, status) => request('PATCH', `/orders/${id}/status`, { status }),
    deleteOrder: (id) => request('DELETE', `/orders/${id}`),

    // Discount
    previewDiscount: (strategy, subTotal, itemCount) =>
        request('POST', '/orders/discount/preview', { strategy, subTotal, itemCount }),
    getDiscountStrategies: () => request('GET', '/orders/discount/strategies'),

    // Customers
    getCustomers: () => request('GET', '/customers'),
};