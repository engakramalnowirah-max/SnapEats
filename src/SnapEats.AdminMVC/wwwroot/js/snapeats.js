// ==========================================
// SnapEats Admin Dashboard - JavaScript (Kendo UI)
// ==========================================

var confirmWindow = null;

$(document).ready(function () {
    'use strict';

    // ==========================================
    // Kendo Notification
    // ==========================================
    var notification = $("#notificationWidget").kendoNotification({
        position: { top: 20, right: 20 },
        stacking: "down",
        autoCloseAfter: 5000,
        templates: [{
            type: "success",
            template: "<div class='k-notification-wrap'><i class='fas fa-check-circle me-2'></i> #= message #</div>"
        }, {
            type: "error",
            template: "<div class='k-notification-wrap'><i class='fas fa-exclamation-circle me-2'></i> #= message #</div>"
        }]
    }).data("kendoNotification");

    // Show notifications from TempData
    $("[id$='Notification']").each(function () {
        var msg = $(this).data("message");
        if (msg) {
            var type = $(this).attr("id").indexOf("success") > -1 ? "success" : "error";
            if (notification) {
                notification.show(msg, type);
            }
        }
    });

    // ==========================================
    // Kendo Confirm Window
    // ==========================================
    if ($("#confirmWindow").length > 0 && typeof $.fn.kendoWindow !== 'undefined') {
        confirmWindow = $("#confirmWindow").kendoWindow({
            width: "420px",
            title: "تأكيد",
            visible: false,
            modal: true,
            actions: ["Close"]
        }).data("kendoWindow");
    }

    // ==========================================
    // Sidebar Hover Expand / Collapse
    // ==========================================
    var $sidebar = $('#sidebar');
    var $wrapper = $('.app-wrapper');

    if ($sidebar.length > 0) {
        $sidebar.on('mouseenter', function () {
            if ($(window).width() > 992) {
                $wrapper.addClass('sidebar-hovered');
                $sidebar.addClass('is-expanded');
            }
        }).on('mouseleave', function () {
            if ($(window).width() > 992) {
                $wrapper.removeClass('sidebar-hovered');
                $sidebar.removeClass('is-expanded');
            }
        });

        $sidebar.on('transitionend webkitTransitionEnd oTransitionEnd', function () {
            if (window.kendo) {
                kendo.resize($(".k-grid, .k-chart"));
            }
            $(window).trigger('resize');
        });
    }

    $('#sidebarToggle').on('click', function () {
        if ($(window).width() <= 992) {
            $('#sidebar').toggleClass('active');
            $('.sidebar-overlay').toggleClass('active');
        } else {
            $wrapper.toggleClass('sidebar-hovered');
            $sidebar.toggleClass('is-expanded');
        }
    });

    $('.sidebar-overlay').on('click', function () {
        $('#sidebar').removeClass('active');
        $(this).removeClass('active');
    });


    // ==========================================
    // Delete Confirmation Modal Fix
    // ==========================================
    $(document).on('click', '.btn-delete', function (e) {
        e.preventDefault();
        var btn = $(this);
        var itemName = btn.attr('data-name') || btn.data('name') || 'العنصر';
        var itemId = btn.attr('data-id') || btn.data('id');

        if (!itemId) {
            console.error('Delete item ID is missing');
            return;
        }

        function executeDelete() {
            var formElem = document.getElementById("deleteForm");
            var inputElem = document.getElementById("deleteId");
            if (formElem && inputElem) {
                inputElem.value = itemId;
                formElem.submit();
            } else {
                console.error("Delete form (#deleteForm) or delete input (#deleteId) missing on page.");
            }
        }

        if (confirmWindow) {
            confirmWindow.content(
                "<div class='text-center' style='padding: 10px 0;'>" +
                "<i class='fas fa-exclamation-triangle' style='font-size: 3rem; color: #FFA502; margin-bottom: 16px;'></i>" +
                "<h5 class='fw-bold mb-2' style='font-family:Almarai;'>تأكيد الحذف</h5>" +
                "<p class='text-muted' style='font-family:Almarai;'>هل أنت متأكد من حذف \"" + itemName + "\"? لا يمكن التراجع عن هذا الإجراء.</p>" +
                "<div style='display:flex; gap: 10px; justify-content: center; padding: 16px 0;'>" +
                "<button type='button' class='k-button k-button-md k-rounded-md k-primary confirm-yes' style='font-family:Almarai;'>نعم، احذف</button>" +
                "<button type='button' class='k-button k-button-md k-rounded-md k-outline confirm-no' style='font-family:Almarai;'>إلغاء</button>" +
                "</div>" +
                "</div>"
            );
            confirmWindow.title("تأكيد الحذف");
            confirmWindow.center().open();

            confirmWindow.element.find(".confirm-yes").off("click").on("click", function () {
                confirmWindow.close();
                executeDelete();
            });

            confirmWindow.element.find(".confirm-no").off("click").on("click", function () {
                confirmWindow.close();
            });
        } else {
            if (confirm("هل أنت متأكد من حذف \"" + itemName + "\"?")) {
                executeDelete();
            }
        }
    });


    // ==========================================
    // Status Change Confirmation
    // ==========================================
    $(document).on('click', '.btn-status-change', function (e) {
        e.preventDefault();
        var form = $(this).closest('form');
        var newStatus = $(this).data('status-text') || 'تحديث الحالة';

        if (confirmWindow) {
            confirmWindow.content(
                "<div class='text-center' style='padding: 10px 0;'>" +
                "<i class='fas fa-question-circle' style='font-size: 3rem; color: #1E90FF; margin-bottom: 16px;'></i>" +
                "<h5 class='fw-bold mb-2' style='font-family:Almarai;'>تأكيد تغيير الحالة</h5>" +
                "<p class='text-muted' style='font-family:Almarai;'>هل أنت متأكد من تغيير حالة الطلب إلى \"" + newStatus + "\"?</p>" +
                "<div style='display:flex; gap: 10px; justify-content: center; padding: 16px 0;'>" +
                "<button type='button' class='k-button k-button-md k-rounded-md k-primary confirm-yes' style='font-family:Almarai;'>نعم، قم بالتغيير</button>" +
                "<button type='button' class='k-button k-button-md k-rounded-md k-outline confirm-no' style='font-family:Almarai;'>إلغاء</button>" +
                "</div>" +
                "</div>"
            );
            confirmWindow.title("تأكيد تغيير الحالة");
            confirmWindow.center().open();

            confirmWindow.element.find(".confirm-yes").off("click").on("click", function () {
                confirmWindow.close();
                form.submit();
            });

            confirmWindow.element.find(".confirm-no").off("click").on("click", function () {
                confirmWindow.close();
            });
        } else {
            if (confirm("هل أنت متأكد من تغيير الحالة؟")) {
                form.submit();
            }
        }
    });

    // ==========================================
    // Cancel Order Confirmation
    // ==========================================
    $(document).on('click', '.btn-cancel-order', function (e) {
        e.preventDefault();
        var form = $(this).closest('form');

        if (confirmWindow) {
            confirmWindow.content(
                "<div class='text-center' style='padding: 10px 0;'>" +
                "<i class='fas fa-exclamation-triangle' style='font-size: 3rem; color: #FF4757; margin-bottom: 16px;'></i>" +
                "<h5 class='fw-bold mb-2' style='font-family:Almarai;'>تأكيد إلغاء الطلب</h5>" +
                "<p class='text-muted' style='font-family:Almarai;'>هل أنت متأكد من إلغاء هذا الطلب?</p>" +
                "<div style='display:flex; gap: 10px; justify-content: center; padding: 16px 0;'>" +
                "<button type='button' class='k-button k-button-md k-rounded-md k-danger confirm-yes' style='font-family:Almarai;'>نعم، ألغ الطلب</button>" +
                "<button type='button' class='k-button k-button-md k-rounded-md k-outline confirm-no' style='font-family:Almarai;'>عودة</button>" +
                "</div>" +
                "</div>"
            );
            confirmWindow.title("تأكيد إلغاء الطلب");
            confirmWindow.center().open();

            confirmWindow.element.find(".confirm-yes").off("click").on("click", function () {
                confirmWindow.close();
                form.submit();
            });

            confirmWindow.element.find(".confirm-no").off("click").on("click", function () {
                confirmWindow.close();
            });
        } else {
            if (confirm("هل أنت متأكد من إلغاء هذا الطلب؟")) {
                form.submit();
            }
        }
    });

    // ==========================================
    // Image Preview
    // ==========================================
    $('input[type="url"][data-preview]').on('change keyup', function () {
        var previewId = $(this).data('preview');
        var url = $(this).val();
        var $preview = $('#' + previewId);

        if (url && url.match(/\.(jpeg|jpg|gif|png|webp)$/i)) {
            $preview.attr('src', url).show();
        } else {
            $preview.hide();
        }
    });

    // ==========================================
    // Auto-dismiss Alerts
    // ==========================================
    setTimeout(function () {
        $('.alert-dismissible').fadeOut('slow');
    }, 5000);
});

// ==========================================
// Utility Functions
// ==========================================
function formatCurrency(amount) {
    return new Intl.NumberFormat('ar-SA', {
        style: 'currency',
        currency: 'SAR'
    }).format(amount);
}

function formatDate(dateString) {
    var date = new Date(dateString);
    return date.toLocaleDateString('ar-SA', {
        year: 'numeric',
        month: 'long',
        day: 'numeric',
        hour: '2-digit',
        minute: '2-digit'
    });
}