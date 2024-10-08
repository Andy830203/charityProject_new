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
        if (callback && typeof callback === 'function') {
            callback();
        }
    }).fail(function (e) {
        alert(e.responseText);
    });
}

function ajaxInit(action, callback) {
    // 選擇所有需要綁定事件的動態內容元素
    let ajaxContents = $(`.ajax${action}`);

    ajaxContents.each(function () {
        let id = $(this).data('id'); // 確保在此元素上設置了 data-id 屬性
        console.log(action + id);
        let url = `/Events/${action}/` + id;

        ajaxGetCall(this, url, callback);
    });
}

// 在頁面加載時初始化內容，或使用適當的事件來調用 loadAccordionContent()
$(window).on('load', function () {
    ajaxInit('EventLocations', bindDragEvents);
    ajaxInit('EventImgs');
    ajaxInit('EventPeriods');
});