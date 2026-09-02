/**
 * SnapEats Admin Real-Time SignalR Notification & DOM Update Script
 */
(function () {
    'use strict';

    // Target API SignalR Hub Endpoint
    const baseUrl = (window.SNAP_EATS_API_URL || "http://localhost:5065").replace(/\/$/, "");
    const apiHubUrl = baseUrl + "/hubs/order";

    if (typeof signalR === 'undefined') {
        console.warn("SignalR library not loaded. Real-time updates will be unavailable.");
        return;
    }

    console.log("Connecting to SnapEats OrderHub at:", apiHubUrl);

    // Build SignalR Hub Connection (no token required)
    const connectionBuilder = new signalR.HubConnectionBuilder()
        .withUrl(apiHubUrl)
        .withAutomaticReconnect([0, 1000, 3000, 5000, 10000, 30000])
        .configureLogging(signalR.LogLevel.Information);

    const connection = connectionBuilder.build();

    // Sound chimes
    function playNotificationSound() {
        try {
            const audioCtx = new (window.AudioContext || window.webkitAudioContext)();
            const osc = audioCtx.createOscillator();
            const gain = audioCtx.createGain();
            osc.type = 'sine';
            osc.frequency.setValueAtTime(587.33, audioCtx.currentTime);
            gain.gain.setValueAtTime(0.15, audioCtx.currentTime);
            osc.connect(gain);
            gain.connect(audioCtx.destination);
            osc.start();
            osc.stop(audioCtx.currentTime + 0.35);
        } catch (e) {
            // Audio context disabled
        }
    }

    function showToast(message, type = "info") {
        const notificationWidget = $("#notificationWidget").data("kendoNotification");
        if (notificationWidget) {
            notificationWidget.show(message, type);
        } else if (window.console) {
            console.log(`[Real-Time ${type.toUpperCase()}]`, message);
        }
    }

    // Helper to format date
    function formatDate(dateStr) {
        if (!dateStr) return new Date().toLocaleString('ar-SA');
        try {
            const d = new Date(dateStr);
            return d.toLocaleDateString('ar-SA') + ' ' + d.toLocaleTimeString('ar-SA', { hour: '2-digit', minute: '2-digit' });
        } catch (e) {
            return dateStr;
        }
    }

    // 1. EVENT: OrderCreated
    connection.on("OrderCreated", function (data) {
        console.log("⚡ SignalR Real-Time Event: OrderCreated", data);
        playNotificationSound();

        const orderId = data.orderId || data.Id || data.id;
        const customerName = data.customerName || data.CustomerName || 'عميل جديد';
        const totalAmount = (data.totalAmount !== undefined ? data.totalAmount : data.TotalAmount) || 0;
        const itemCount = data.itemCount || data.ItemCount || 1;
        const orderDate = data.orderDate || data.OrderDate || new Date().toISOString();
        const status = data.status || data.Status || "Pending";

        showToast(`طلب جديد رقم #${orderId} بقيمة ${totalAmount} ريال من ${customerName}`, "success");

        if (!window.$) return;

        // A. Update Orders Page - KendoGrid
        const ordersGrid = $("#ordersGrid");
        if (ordersGrid.length > 0) {
            const grid = ordersGrid.data("kendoGrid");
            if (grid) {
                grid.dataSource.insert(0, {
                    Id: orderId,
                    customerName: customerName,
                    orderDate: new Date(orderDate),
                    itemCount: itemCount,
                    totalAmount: totalAmount,
                    status: status
                });
                ordersGrid.show();
                $("#ordersTable, #ordersEmptyState").hide();
            } else {
                // Grid element exists but not initialized — show fallback table
                const tbody = $("#ordersTable tbody");
                if (tbody.length === 0) {
                    const rowHtml = buildOrderRowHtml(orderId, customerName, orderDate, itemCount, totalAmount, status);
                    const tableBody = `<tbody>${rowHtml}</tbody>`;
                    $("#ordersTable").html(`<div class="table-container"><table class="table"><thead><tr><th>رقم الطلب</th><th>العميل</th><th>تاريخ الطلب</th><th>عدد العناصر</th><th>المجموع الكلي</th><th>الحالة</th><th>الإجراءات</th></tr></thead>${tableBody}</table></div>`);
                    $("#ordersTable").show();
                    $("#ordersEmptyState").hide();
                } else {
                    tbody.prepend(buildOrderRowHtml(orderId, customerName, orderDate, itemCount, totalAmount, status));
                    $("#ordersTable").show();
                    $("#ordersEmptyState").hide();
                }
            }
        }

        // B. Update Dashboard Page - Recent Orders Grid
        const recentOrdersGrid = $("#recentOrdersGrid");
        if (recentOrdersGrid.length > 0) {
            const recentGrid = recentOrdersGrid.data("kendoGrid");
            if (recentGrid) {
                recentGrid.dataSource.insert(0, {
                    Id: orderId,
                    customerName: customerName,
                    orderDate: new Date(orderDate),
                    itemCount: itemCount,
                    totalAmount: totalAmount,
                    status: status
                });
                recentOrdersGrid.show();
                $("#recentOrdersTable, #recentOrdersEmptyState").hide();
            }
        }

        // C. Update Dashboard Page - HTML Table
        const recentOrdersTable = $("#recentOrdersTable");
        if (recentOrdersTable.length > 0) {
            const tbody = recentOrdersTable.find("tbody");
            if (tbody.length > 0) {
                tbody.prepend(buildRecentOrderRowHtml(orderId, customerName, orderDate, totalAmount, status));
                recentOrdersTable.show();
                $("#recentOrdersEmptyState").hide();
            }
        }

        // D. Update Dashboard Counters
        $(".stat-card").each(function () {
            const text = $(this).text();
            if (text.includes("إجمالي الطلبات")) {
                const valElem = $(this).find(".stat-value");
                const count = parseInt(valElem.text().replace(/[^0-9]/g, '')) || 0;
                valElem.text(count + 1);
            }
            if (text.includes("الطلبات المعلقة")) {
                const valElem = $(this).find(".stat-value");
                const count = parseInt(valElem.text().replace(/[^0-9]/g, '')) || 0;
                valElem.text(count + 1);
            }
        });
    });

    function buildOrderRowHtml(orderId, customerName, orderDate, itemCount, totalAmount, status) {
        return `<tr class="table-success highlight-new-row" id="order-row-${orderId}">
            <td><strong>#${orderId}</strong></td>
            <td><strong class="text-secondary">${customerName}</strong></td>
            <td>${formatDate(orderDate)}</td>
            <td>${itemCount} عناصر</td>
            <td><strong class="text-primary">${Number(totalAmount).toFixed(2)}</strong> ريال</td>
            <td><span class="k-chip k-chip-solid-warning">قيد الانتظار</span></td>
            <td>
                <div style="display:flex;gap:6px;align-items:center;">
                    <a href="/Admin/Orders/Details/${orderId}" class="k-button k-button-md k-rounded-md k-info"><i class="fas fa-eye me-1"></i> عرض</a>
                    <a href="/Admin/Orders/Edit/${orderId}" class="k-button k-button-md k-rounded-md k-warning"><i class="fas fa-edit me-1"></i> تعديل</a>
                </div>
            </td>
        </tr>`;
    }

    function buildRecentOrderRowHtml(orderId, customerName, orderDate, totalAmount, status) {
        return `<tr class="table-success highlight-new-row" id="recent-order-row-${orderId}">
            <td><strong>#${orderId}</strong></td>
            <td>${customerName}</td>
            <td>${formatDate(orderDate)}</td>
            <td><strong class="text-primary">${Number(totalAmount).toFixed(2)}</strong> ريال</td>
            <td><span class="k-chip k-chip-solid-warning">قيد الانتظار</span></td>
            <td>
                <a href="/Admin/Orders/Details/${orderId}" class="k-button k-button-md k-rounded-md k-info"><i class="fas fa-eye me-1"></i> عرض</a>
            </td>
        </tr>`;
    }

    // 2. EVENT: OrderStatusChanged - handles Orders page, Dashboard, AND Details page
    connection.on("OrderStatusChanged", function (data) {
        console.log("⚡ SignalR Real-Time Event: OrderStatusChanged", data);
        const orderId = data.orderId || data.OrderId || data.Id;
        const newStatus = data.newStatus || data.NewStatus || data.status;

        const statusMap = {
            "Pending": "قيد الانتظار",
            "Confirmed": "مؤكد",
            "Preparing": "قيد التحضير",
            "OutForDelivery": "قيد التوصيل",
            "Delivered": "تم التوصيل",
            "Cancelled": "ملغي"
        };
        const statusColors = {
            "Pending": "warning",
            "Confirmed": "info",
            "Preparing": "primary",
            "OutForDelivery": "secondary",
            "Delivered": "success",
            "Cancelled": "danger"
        };

        const statusText = statusMap[newStatus] || newStatus;
        const chipColor = statusColors[newStatus] || "secondary";

        showToast(`تم تحديث حالة الطلب #${orderId} إلى: ${statusText}`, "info");

        if (window.$) {
            // A. Update Orders KendoGrid
            if ($("#ordersGrid").length > 0 && $("#ordersGrid").data("kendoGrid")) {
                const grid = $("#ordersGrid").data("kendoGrid");
                const dataItem = grid.dataSource.get(orderId);
                if (dataItem) {
                    dataItem.set("status", newStatus);
                } else {
                    grid.dataSource.read();
                }
            }

            // B. Update Dashboard Recent Orders KendoGrid
            if ($("#recentOrdersGrid").length > 0 && $("#recentOrdersGrid").data("kendoGrid")) {
                const recentGrid = $("#recentOrdersGrid").data("kendoGrid");
                const dataItem = recentGrid.dataSource.get(orderId);
                if (dataItem) {
                    dataItem.set("status", newStatus);
                }
            }

            // C. Update HTML Table chips (Orders + Dashboard recent)
            $(`#order-row-${orderId} .k-chip, #recent-order-row-${orderId} .k-chip`).replaceWith(
                `<span class="k-chip k-chip-solid-${chipColor}">${statusText}</span>`
            );

            // D. Update Details Page if currently viewing this order
            const detailsPage = $(".order-details-page");
            const currentOrderId = detailsPage.data("orderId");

            if (detailsPage.length > 0 && currentOrderId && currentOrderId == orderId) {
                // D1. Update status badge in header
                const statusBadge = detailsPage.find(".order-status-badge");
                if (statusBadge.length > 0) {
                    statusBadge.attr("class", `k-chip k-chip-solid-${chipColor} order-status-badge`);
                    statusBadge.html(`<i class="fas fa-circle me-1" style="font-size:0.6rem;"></i> ${statusText}`);
                }

                // D2. Update action buttons based on new status
                const actionsContainer = detailsPage.find(".order-actions-container");
                if (actionsContainer.length > 0) {
                    if (newStatus === "Delivered" || newStatus === "Cancelled") {
                        actionsContainer.html(`
                            <div class="p-3 rounded border ${newStatus === "Delivered" ? "bg-success-light text-success" : "bg-danger-light text-danger"} font-weight-bold">
                                <i class="fas fa-info-circle me-1"></i>
                                <span>${newStatus === "Delivered" ? "تم تسليم هذا الطلب بنجاح وهو مكتمل." : "تم إلغاء هذا الطلب ولا يمكن تعديله."}</span>
                            </div>
                        `);
                    } else {
                        let buttonsHtml = '<div class="d-flex flex-column gap-2">';

                        if (newStatus === "Pending") {
                            buttonsHtml += `
                                <form asp-area="Admin" asp-controller="Orders" asp-action="UpdateStatus" method="post" class="status-update-form">
                                    @Html.AntiForgeryToken()
                                    <input type="hidden" name="orderId" value="${orderId}" />
                                    <input type="hidden" name="status" value="Preparing" />
                                    <button type="button" class="k-button k-button-md k-rounded-md k-primary w-100 btn-status-change" data-status-text="قيد التحضير">
                                        <i class="fas fa-fire me-1"></i> بدء تحضير الوجبة
                                    </button>
                                </form>`;
                        }
                        if (newStatus === "Preparing") {
                            buttonsHtml += `
                                <form asp-area="Admin" asp-controller="Orders" asp-action="UpdateStatus" method="post" class="status-update-form">
                                    @Html.AntiForgeryToken()
                                    <input type="hidden" name="orderId" value="${orderId}" />
                                    <input type="hidden" name="status" value="Delivered" />
                                    <button type="button" class="k-button k-button-md k-rounded-md k-success w-100 btn-status-change" data-status-text="تم التوصيل">
                                        <i class="fas fa-check-circle me-1"></i> تأكيد استلام العميل
                                    </button>
                                </form>`;
                        }
                        if (newStatus === "OutForDelivery") {
                            buttonsHtml += `
                                <form asp-area="Admin" asp-controller="Orders" asp-action="UpdateStatus" method="post" class="status-update-form">
                                    @Html.AntiForgeryToken()
                                    <input type="hidden" name="orderId" value="${orderId}" />
                                    <input type="hidden" name="status" value="Delivered" />
                                    <button type="button" class="k-button k-button-md k-rounded-md k-success w-100 btn-status-change" data-status-text="تم التوصيل">
                                        <i class="fas fa-check-circle me-1"></i> تأكيد استلام العميل
                                    </button>
                                </form>`;
                        }
                        if (newStatus === "Pending" || newStatus === "Preparing") {
                            buttonsHtml += `
                                <form asp-area="Admin" asp-controller="Orders" asp-action="Cancel" method="post" class="cancel-order-form">
                                    @Html.AntiForgeryToken()
                                    <input type="hidden" name="orderId" value="${orderId}" />
                                    <button type="button" class="k-button k-button-md k-rounded-md k-danger w-100 btn-cancel-order">
                                        <i class="fas fa-ban me-1"></i> إلغاء الطلب
                                    </button>
                                </form>`;
                        }

                        buttonsHtml += '</div>';
                        actionsContainer.html(buttonsHtml);
                    }
                }

                // D3. Update timeline
                const timelineContainer = detailsPage.find(".order-timeline");
                if (timelineContainer.length > 0) {
                    const timelineSteps = {
                        "Confirmed": `
                            <div class="d-flex align-items-center gap-3 timeline-step" data-status="Confirmed">
                                <div class="rounded-circle bg-info text-white d-flex align-items-center justify-content-center" style="width:36px;height:36px;"><i class="fas fa-check"></i></div>
                                <div>
                                    <h6 class="mb-0 fw-bold">تأكيد الطلب</h6>
                                    <small class="text-muted">تم اعتماد الطلب من الإدارة</small>
                                </div>
                            </div>`,
                        "Preparing": `
                            <div class="d-flex align-items-center gap-3 timeline-step" data-status="Preparing">
                                <div class="rounded-circle bg-primary text-white d-flex align-items-center justify-content-center" style="width:36px;height:36px;"><i class="fas fa-fire"></i></div>
                                <div>
                                    <h6 class="mb-0 fw-bold">تحضير الوجبة</h6>
                                    <small class="text-muted">جاري طهي وتجهيز الوجبات المحددة</small>
                                </div>
                            </div>`,
                        "OutForDelivery": `
                            <div class="d-flex align-items-center gap-3 timeline-step" data-status="OutForDelivery">
                                <div class="rounded-circle bg-secondary text-white d-flex align-items-center justify-content-center" style="width:36px;height:36px;"><i class="fas fa-truck"></i></div>
                                <div>
                                    <h6 class="mb-0 fw-bold">قيد التوصيل</h6>
                                    <small class="text-muted">السائق في الطريق لإيصال الوجبة</small>
                                </div>
                            </div>`,
                        "Delivered": `
                            <div class="d-flex align-items-center gap-3 timeline-step" data-status="Delivered">
                                <div class="rounded-circle bg-success text-white d-flex align-items-center justify-content-center" style="width:36px;height:36px;"><i class="fas fa-check-circle"></i></div>
                                <div>
                                    <h6 class="mb-0 fw-bold">اكتمل التوصيل</h6>
                                    <small class="text-muted">تم التسليم النهائي للعميل بنجاح</small>
                                </div>
                            </div>`,
                        "Cancelled": `
                            <div class="d-flex align-items-center gap-3 timeline-step" data-status="Cancelled">
                                <div class="rounded-circle bg-danger text-white d-flex align-items-center justify-content-center" style="width:36px;height:36px;"><i class="fas fa-times"></i></div>
                                <div>
                                    <h6 class="mb-0 fw-bold">تم إلغاء الطلب</h6>
                                    <small class="text-muted">تم إلغاء الطلب</small>
                                </div>
                            </div>`
                    };

                    timelineContainer.find(".timeline-step").remove();

                    const statusOrder = ["Confirmed", "Preparing", "OutForDelivery", "Delivered", "Cancelled"];
                    const currentIndex = statusOrder.indexOf(newStatus);

                    if (currentIndex >= 0) {
                        for (let i = 0; i <= currentIndex; i++) {
                            const stepStatus = statusOrder[i];
                            if (timelineSteps[stepStatus]) {
                                timelineContainer.append(timelineSteps[stepStatus]);
                            }
                        }
                    }
                }
            }
        }
    });


    // 4. EVENT: OrderDeleted
    connection.on("OrderDeleted", function (data) {
        console.log("⚡ SignalR Real-Time Event: OrderDeleted", data);
        const orderId = data.orderId || data.Id;
        showToast(`تم حذف الطلب #${orderId}`, "warning");
        if (window.$) {
            $(`#order-row-${orderId}, #recent-order-row-${orderId}`).fadeOut('slow', function() { $(this).remove(); });
        }
    });

    // 5. EVENT: CategoryChanged
    connection.on("CategoryChanged", function (data) {
        console.log("⚡ SignalR Real-Time Event: CategoryChanged", data);
        const action = data.action || data.Action;
        const name = data.name || data.Name;
        showToast(`تغيير في التصنيفات (${action}): ${name}`, "info");

        if (window.$) {
            if ($("#categoriesGrid").length > 0 && $("#categoriesGrid").data("kendoGrid")) {
                $("#categoriesGrid").data("kendoGrid").dataSource.read();
            }
        }
    });

    // 6. EVENT: MenuItemChanged
    connection.on("MenuItemChanged", function (data) {
        console.log("⚡ SignalR Real-Time Event: MenuItemChanged", data);
        const action = data.action || data.Action;
        const name = data.name || data.Name;
        const isAvailable = data.isAvailable !== undefined ? data.isAvailable : data.IsAvailable;
        const availabilityText = isAvailable === false ? ' (غير متوفر)' : '';
        showToast(`تحديث في قائمة الطعام (${action}): ${name}${availabilityText}`, "info");

        if (window.$) {
            if ($("#menuItemsGrid").length > 0 && $("#menuItemsGrid").data("kendoGrid")) {
                $("#menuItemsGrid").data("kendoGrid").dataSource.read();
            }
        }
    });


    // Start SignalR Connection with Retry Logic
    async function startConnection() {
        try {
            await connection.start();
            console.log("%c[SnapEats SignalR] Connected successfully to OrderHub!", "color: green; font-weight: bold;");
        } catch (err) {
            console.error("[SnapEats SignalR] Connection failed: ", err);
            setTimeout(startConnection, 4000);
        }
    }

    connection.onclose(function (error) {
        console.warn("[SnapEats SignalR] Connection lost. Attempting reconnect...", error);
    });

    if (document.readyState === "loading") {
        document.addEventListener("DOMContentLoaded", startConnection);
    } else {
        startConnection();
    }
})();
