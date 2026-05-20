$(function () {
    initAutocomplete();
    initDateTimePickers();
    initConfirmModal();
    initDeleteForms();
});

function initAutocomplete() {
    $('.autocomplete-wrapper').each(function () {
        var wrapper = $(this);
        var input = wrapper.find('.autocomplete-input');
        var hidden = wrapper.find('input[type=hidden]');
        var dropdown = wrapper.find('.autocomplete-dropdown');
        var url = wrapper.data('search-url');
        var timer;

        input.on('input', function () {
            clearTimeout(timer);
            var q = $(this).val();
            if (q.length < 2) {
                dropdown.removeClass('open').empty();
                return;
            }
            timer = setTimeout(function () {
                $.get(url, { q: q }, function (data) {
                    dropdown.empty();
                    if (data.length === 0) {
                        dropdown.removeClass('open');
                        return;
                    }
                    data.forEach(function (item) {
                        var display = item.username || item.name || item.label || item.id;
                        var sub = item.email ? ' (' + item.email + ')' : '';
                        dropdown.append(
                            '<div class="autocomplete-item" data-id="' + item.id + '">' + display + sub + '</div>'
                        );
                    });
                    dropdown.addClass('open');
                });
            }, 250);
        });

        dropdown.on('click', '.autocomplete-item', function () {
            var item = $(this);
            hidden.val(item.data('id'));
            input.val(item.text().trim());
            dropdown.removeClass('open').empty();
        });

        $(document).on('click', function (e) {
            if (!$(e.target).closest(wrapper).length) {
                dropdown.removeClass('open');
            }
        });

        input.on('keydown', function (e) {
            var items = dropdown.find('.autocomplete-item');
            var selected = items.filter('.selected');
            if (e.key === 'ArrowDown') {
                e.preventDefault();
                if (selected.length === 0) items.first().addClass('selected');
                else { selected.removeClass('selected').next().addClass('selected'); }
            } else if (e.key === 'ArrowUp') {
                e.preventDefault();
                if (selected.length === 0) items.last().addClass('selected');
                else { selected.removeClass('selected').prev().addClass('selected'); }
            } else if (e.key === 'Enter') {
                e.preventDefault();
                if (selected.length) selected.click();
            } else if (e.key === 'Escape') {
                dropdown.removeClass('open');
            }
        });
    });
}

function initDateTimePickers() {
    var locale = document.documentElement.lang || 'en';
    if (typeof flatpickr !== 'undefined') {
        flatpickr('.datetime-picker', {
            enableTime: true,
            dateFormat: 'Y-m-d H:i',
            time_24hr: true,
            locale: locale === 'hr' ? 'hr' : 'default',
            theme: 'dark'
        });
    }
}

function initConfirmModal() {
    window.showConfirm = function (message, onConfirm) {
        var overlay = $('#confirm-modal');
        $('#confirm-modal-message').text(message);
        overlay.addClass('visible');
        $('#confirm-modal-ok').off('click').on('click', function () {
            overlay.removeClass('visible');
            if (onConfirm) onConfirm();
        });
        $('#confirm-modal-cancel').off('click').on('click', function () {
            overlay.removeClass('visible');
        });
    };
}

function initDeleteForms() {
    $(document).on('click', '.delete-form button[data-confirm]', function (e) {
        e.preventDefault();
        var btn = $(this);
        var form = btn.closest('form');
        var msg = btn.data('confirm') || 'Are you sure?';
        if (window.showConfirm) {
            showConfirm(msg, function () {
                form.off('submit').submit();
            });
        }
    });
}

function showToast(message, type) {
    type = type || 'info';
    var container = $('#toast-container');
    var toast = $('<div class="toast toast-' + type + '">' + message + '</div>');
    container.append(toast);
    setTimeout(function () {
        toast.remove();
    }, 3000);
}