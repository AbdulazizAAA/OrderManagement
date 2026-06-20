const apiUrl = "/api/orders";

let searchInput;
let ordersTableBody;
let selectedOrderId = null;

document.addEventListener("DOMContentLoaded", () => {

    searchInput =
        document.getElementById("txtSearch");

    ordersTableBody =
        document.querySelector("#ordersTable tbody");

    if (!searchInput || !ordersTableBody) {
        console.error("Required DOM elements not found");
        return;
    }

    loadOrders();

    searchInput.addEventListener(
        "input",
        debounce(searchOrders, 500)
    );
});
async function loadOrders() {

    try {

        const response =
            await fetch(apiUrl);

        if (!response.ok)
            throw new Error("Failed to load orders");

        const orders =
            await response.json();

        renderOrders(orders);

    } catch (err) {

        console.error(err);

        alert("Error loading orders");
    }
}
async function searchOrders() {

    try {

        const search = searchInput.value || "";

        const response =
            await fetch(
                `${apiUrl}?search=${encodeURIComponent(search)}`
            );

        if (!response.ok)
            throw new Error("Search failed");

        const orders =
            await response.json();

        renderOrders(orders);

    } catch (err) {

        console.error(err);
    }
}
function renderOrders(orders) {

    ordersTableBody.replaceChildren();

    if (!orders || orders.length === 0) {

        const emptyRow =
            document.createElement("tr");

        const emptyCell =
            document.createElement("td");

        emptyCell.colSpan = 6;
        emptyCell.textContent = "No orders found";

        emptyRow.appendChild(emptyCell);
        ordersTableBody.appendChild(emptyRow);

        return;
    }

    orders.forEach(order => {

        const row = document.createElement("tr");

        appendCell(row, order.customerName);
        appendCell(row, order.items?.length ?? 0);
        appendCell(row, order.subTotal);
        appendCell(row, order.discount);
        appendCell(row, order.total);

        const actionCell = document.createElement("td");

        const deleteBtn = document.createElement("button");
        deleteBtn.textContent = "Delete";

        deleteBtn.addEventListener("click", () => {
            confirmDelete(order.id);
        });

        actionCell.appendChild(deleteBtn);
        row.appendChild(actionCell);

        ordersTableBody.appendChild(row);
    });
}
function appendCell(row, value) {

    const cell = document.createElement("td");
    cell.textContent = value ?? "";
    row.appendChild(cell);
}
function confirmDelete(id) {

    selectedOrderId = id;

    const ok =
        window.confirm("Are you sure you want to delete this order?");

    if (ok)
        deleteOrder();
}
async function deleteOrder() {

    try {

        const response =
            await fetch(`${apiUrl}/${selectedOrderId}`, {
                method: "DELETE"
            });

        if (!response.ok)
            throw new Error("Delete failed");

        await loadOrders();

    } catch (err) {

        console.error(err);
        alert("Error deleting order");
    }
}
function debounce(callback, delay) {

    let timerId;

    return (...args) => {

        clearTimeout(timerId);

        timerId =
            setTimeout(() => {
                callback(...args);
            }, delay);
    };
}