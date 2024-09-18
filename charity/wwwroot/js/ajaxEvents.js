// function ajaxGetCall(elementId, url) {
//     $.ajax({
//         type: 'GET',
//         url: url
//     }).done(function (result) {
//         $(elementId).html(result);
//     }).fail(function (e) {
//         alert(e.responseText);
//     });
// }

// function bindingOnchangeEvent(id) {
//     // let elementId = content + id;
//     $(this).on('change', function () {
//         let urlAction = '/Events/EventLocations/';
//         ajaxGetCall(this, urlAction+id);
//     });
// }
/* from chatGPT */
function ajaxGetCall(elementId, url, callback) {
    $.ajax({
        type: 'GET',
        url: url
    }).done(function (result) {
        $(elementId).html(result);
        if (callback) callback(); // 如果提供了回呼函數，就執行它
    }).fail(function (e) {
        alert(e.responseText);
    });
}

function bindingOnchangeEvent(element) {
    $(element).on('change', function () {
        let id = $(this).data('id'); // 從元素的 data 屬性中獲取 ID
        let urlAction = '/Events/EventLocations/';
        ajaxGetCall(this, urlAction + id);
    });
}

// 在內容加載後調用此函數來綁定事件
function loadAccordionContent() {
    // 選擇所有需要綁定事件的動態內容元素
    let ajaxContents = $('.ajax-contents');

    ajaxContents.each(function () {
        let id = $(this).data('id'); // 確保在此元素上設置了 data-id 屬性
        let url = '/Events/EventLocations/' + id;

        ajaxGetCall(this, url, function () {
            // 內容加載後綁定事件
            bindingOnchangeEvent(this);
        }.bind(this));
    });
}

// 在頁面加載時初始化內容，或使用適當的事件來調用 loadAccordionContent()
document.addEventListener('DOMContentLoaded', function () {
    loadAccordionContent();
});