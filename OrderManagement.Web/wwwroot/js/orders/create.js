let itemsBody;
let discountType;

let subTotalElement;
let discountElement;
let totalElement;

document.addEventListener("DOMContentLoaded", () => {

    itemsBody =
        document.getElementById("itemsBody");

    discountType =
        document.getElementById("discountType");

    subTotalElement =
        document.getElementById("subTotal");

    discountElement =
        document.getElementById("discountAmount");

    totalElement =
        document.getElementById("grandTotal");

    const addBtn =
        document.getElementById("addItemBtn");

    if (!addBtn) {
        console.error("btnAddItem not found in DOM");
        return;
    }

    addBtn.addEventListener("click", addItemRow);

    const btnSaveOrder =
        document.getElementById("btnSaveOrder");
    if (!btnSaveOrder) {
        console.error("btnSaveOrder not found in DOM");
        return;
    }
    btnSaveOrder.addEventListener("click", saveOrder);

    discountType.addEventListener(
        "change",
        calculateTotals
    );

    addItemRow(); // initial row
});


function createInput(type, cssClass) {

    const input =
        document.createElement("input");

    input.type = type;

    input.classList.add(cssClass);

    return input;
}

function createCell(element) {

    const cell =
        document.createElement("td");

    cell.appendChild(element);

    return cell;
}

function addItemRow() {

    const row =
        document.createElement("tr");

    const productInput =
        createInput("text", "product");

    const priceInput =
        createInput("number", "price");

    const qtyInput =
        createInput("number", "qty");

    const totalCell =
        document.createElement("td");

    totalCell.classList.add("total");
    totalCell.textContent = "0";

    const removeButton =
        document.createElement("button");

    removeButton.type = "button";
    removeButton.textContent = "Remove";

    const actionCell =
        document.createElement("td");

    actionCell.appendChild(removeButton);

    row.appendChild(createCell(productInput));
    row.appendChild(createCell(priceInput));
    row.appendChild(createCell(qtyInput));
    row.appendChild(totalCell);
    row.appendChild(actionCell);

    itemsBody.appendChild(row);

    priceInput.addEventListener(
        "input",
        calculateTotals);

    qtyInput.addEventListener(
        "input",
        calculateTotals);

    removeButton.addEventListener(
        "click",
        () => {

            row.remove();

            calculateTotals();
        });
}

async function calculateTotals() {

    let subTotal = 0;

    const rows =
        itemsBody.querySelectorAll("tr");

    rows.forEach(row => {

        const price =
            Number(
                row.querySelector(".price")
                    .value || 0);

        const qty =
            Number(
                row.querySelector(".qty")
                    .value || 0);

        const rowTotal =
            price * qty;

        row.querySelector(".total")
            .textContent = rowTotal;

        subTotal += rowTotal;
    });

    subTotalElement.textContent =
        subTotal.toFixed(2);

    let discount = 0;

    const selectedStrategy =
        discountType.value;

    if (selectedStrategy === "Percentage")
        discount = subTotal * 0.10;

    if (selectedStrategy === "Fixed")
        discount = 100;

    discountElement.textContent =
        discount.toFixed(2);

    totalElement.textContent =
        (subTotal - discount).toFixed(2);
}

async function saveOrder() {

    const customerId =
        document.getElementById("customerId").value;

    const items = [];

    const rows = itemsBody.querySelectorAll("tr");

    rows.forEach(row => {

        items.push({
            productName: row.querySelector(".product").value,
            unitPrice: Number(row.querySelector(".price").value),
            quantity: Number(row.querySelector(".qty").value)
        });
    });

    const request = {

        customerId: customerId,

        discountStrategy: discountType.value,

        items: items
    };

    try {

        const response = await fetch("/Orders/Create", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(request)
        });

        if (!response.ok)
            throw new Error("Failed to save order");

        window.location.href = "/Orders";

    } catch (err) {

        console.error(err);
        alert("Failed to save order");
    }
}
