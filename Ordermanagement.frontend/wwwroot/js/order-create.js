// order-create.js - Create order page: dynamic items, discount preview, validation
import { api } from './api.js';
import { formatCurrency, showToast, validateField, debounce } from './utils.js';

const form = document.getElementById('orderForm');
const itemsContainer = document.getElementById('itemsContainer');
const addItemBtn = document.getElementById('addItemBtn');
const submitBtn = document.getElementById('submitBtn');
const submitText = document.getElementById('submitText');
const submitSpinner = document.getElementById('submitSpinner');
const summaryBox = document.getElementById('summaryBox');
const summarySubtotal = document.getElementById('summarySubtotal');
const summaryDiscount = document.getElementById('summaryDiscount');
const summaryTotal = document.getElementById('summaryTotal');
const discountRow = document.getElementById('discountRow');
const discountLabel = document.getElementById('discountLabel');
const itemsError = document.getElementById('itemsError');
const template = document.getElementById('itemRowTemplate');

let itemIndex = 0;
let discountPreview = null;

// ---- Item Management ----

function addItem() {
    const idx = itemIndex++;
    const html = template.innerHTML
        .replaceAll('__INDEX__', idx)
        .replaceAll('__NUM__', itemsContainer.children.length + 1);

    const wrapper = document.createElement('div');
    wrapper.innerHTML = html;
    const row = wrapper.firstElementChild;
    itemsContainer.appendChild(row);

    // Remove button
    row.querySelector('.remove-item-btn')?.addEventListener('click', () => {
        row.remove();
        renumberItems();
        updateSummary();
        updateSubmitState();
    });

    // Live update on field change
    row.querySelectorAll('input').forEach(input => {
        input.addEventListener('input', () => {
            updateLineTotal(row);
            updateSummary();
            updateSubmitState();
        });
        input.addEventListener('blur', () => validateField(input));
    });

    updateSubmitState();
    row.querySelector('.item-product-name')?.focus();
}

function renumberItems() {
    [...itemsContainer.children].forEach((row, i) => {
        const label = row.querySelector('.item-number');
        if (label) label.textContent = `Item ${i + 1}`;
        row.dataset.index = i;
    });
}

function updateLineTotal(row) {
    const qty = parseFloat(row.querySelector('.item-quantity')?.value) || 0;
    const price = parseFloat(row.querySelector('.item-unit-price')?.value) || 0;
    const total = qty * price;
    const el = row.querySelector('.item-line-total');
    if (el) el.textContent = formatCurrency(total);
}

// ---- Summary & Discount Preview ----

async function updateSummary() {
    const items = getItems();
    const subTotal = items.reduce((s, i) => s + i.quantity * i.unitPrice, 0);
    const itemCount = items.reduce((s, i) => s + i.quantity, 0);
    const strategy = getSelectedStrategy();

    if (summarySubtotal) summarySubtotal.textContent = formatCurrency(subTotal);
    summaryBox.hidden = items.length === 0;

    if (subTotal > 0 && strategy !== null) {
        try {
            discountPreview = await api.previewDiscount(strategy, subTotal, itemCount);
            if (discountRow) {
                if (discountPreview.discountAmount > 0) {
                    discountRow.hidden = false;
                    if (discountLabel) discountLabel.textContent = discountPreview.strategyName;
                    if (summaryDiscount) summaryDiscount.textContent = `-${formatCurrency(discountPreview.discountAmount)}`;
                } else {
                    discountRow.hidden = true;
                }
            }
            if (summaryTotal) summaryTotal.textContent = formatCurrency(discountPreview.totalAmount);
            updatePreviewPanel(items, discountPreview);
        } catch {
            if (summaryTotal) summaryTotal.textContent = formatCurrency(subTotal);
        }
    } else {
        if (discountRow) discountRow.hidden = true;
        if (summaryTotal) summaryTotal.textContent = formatCurrency(subTotal);
    }
}

const debouncedUpdateSummary = debounce(updateSummary, 400);

function updatePreviewPanel(items, preview) {
    const panel = document.getElementById('previewContent');
    if (!panel) return;

    panel.innerHTML = `
    <ul class="preview-items">
      ${items.filter(i => i.productName).map(i => `
        <li class="preview-item">
          <span>${i.productName || 'Unnamed'}</span>
          <span>${i.quantity} × ${formatCurrency(i.unitPrice)}</span>
        </li>`).join('')}
    </ul>
    <div class="preview-totals">
      <div class="preview-row"><span>Subtotal</span><span>${formatCurrency(preview.subTotal)}</span></div>
      ${preview.discountAmount > 0 ? `<div class="preview-row preview-discount"><span>${preview.strategyName}</span><span>-${formatCurrency(preview.discountAmount)}</span></div>` : ''}
      <div class="preview-row preview-grand-total"><span>Total</span><span>${formatCurrency(preview.totalAmount)}</span></div>
    </div>`;
}

// ---- Form Helpers ----

function getItems() {
    return [...itemsContainer.querySelectorAll('.item-row')].map(row => ({
        productName: row.querySelector('.item-product-name')?.value.trim() ?? '',
        productCode: row.querySelector('.item-product-code')?.value.trim() ?? '',
        quantity: parseInt(row.querySelector('.item-quantity')?.value) || 0,
        unitPrice: parseFloat(row.querySelector('.item-unit-price')?.value) || 0,
    }));
}

function getSelectedStrategy() {
    const radio = form?.querySelector('.strategy-radio:checked');
    return radio ? parseInt(radio.value) : 0;
}

function updateSubmitState() {
    const hasItems = itemsContainer.children.length > 0;
    const customerId = document.getElementById('customerId')?.value;
    if (submitBtn) submitBtn.disabled = !hasItems || !customerId;
    if (itemsError) itemsError.textContent = !hasItems ? 'Please add at least one item.' : '';
}

function validateForm() {
    let valid = true;
    const customerId = document.getElementById('customerId');
    if (!customerId?.value) {
        customerId.classList.add('input-error');
        document.getElementById('customerIdError').textContent = 'Please select a customer.';
        valid = false;
    } else {
        customerId.classList.remove('input-error');
        document.getElementById('customerIdError').textContent = '';
    }

    form.querySelectorAll('input[required]').forEach(input => {
        if (!validateField(input)) valid = false;
    });

    if (itemsContainer.children.length === 0) {
        if (itemsError) itemsError.textContent = 'Please add at least one item.';
        valid = false;
    }

    return valid;
}

// ---- Submit ----

async function handleSubmit(e) {
    e.preventDefault();
    if (!validateForm()) return;

    submitBtn.disabled = true;
    submitText.textContent = 'Creating...';
    submitSpinner.hidden = false;

    const payload = {
        customerId: document.getElementById('customerId').value,
        discountStrategy: getSelectedStrategy(),
        items: getItems(),
    };

    try {
        const order = await api.createOrder(payload);
        showToast(`Order ${order.orderNumber} created!`, 'success');
        setTimeout(() => window.location.href = `/Orders/${order.id}`, 800);
    } catch (err) {
        const errEl = document.getElementById('serverErrors');
        if (errEl) {
            errEl.hidden = false;
            const errors = err.body?.errors
                ? Object.values(err.body.errors).flat().map(e => `<li>${e}</li>`).join('')
                : `<li>${err.message}</li>`;
            errEl.innerHTML = `<ul>${errors}</ul>`;
            errEl.scrollIntoView({ behavior: 'smooth' });
        }
        showToast('Failed to create order.', 'error');
        submitBtn.disabled = false;
        submitText.textContent = 'Create Order';
        submitSpinner.hidden = true;
    }
}

// ---- Init ----

document.addEventListener('DOMContentLoaded', () => {
    addItemBtn?.addEventListener('click', addItem);
    form?.addEventListener('submit', handleSubmit);

    // Strategy change triggers discount recalc
    form?.querySelectorAll('.strategy-radio').forEach(r => {
        r.addEventListener('change', () => {
            // Visually update strategy cards
            form.querySelectorAll('.strategy-card').forEach(c => c.classList.remove('selected'));
            r.closest('.strategy-card')?.classList.add('selected');
            debouncedUpdateSummary();
        });
    });

    // Customer change updates submit state
    document.getElementById('customerId')?.addEventListener('change', () => {
        updateSubmitState();
    });

    // Add 2 default items
    addItem();
    addItem();
});