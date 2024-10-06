function enableDragAndDrop(tableBody) {
    // 檢查是否已經綁定過拖放功能，避免重複綁定
    if (tableBody.getAttribute('data-drag-bound')) {
        return; // 已經綁定過則跳過
    }

    let draggedRow = null;

    tableBody.addEventListener('dragstart', function (event) {
        draggedRow = event.target; // 紀錄被拖曳的元素
        event.dataTransfer.effectAllowed = "move";
        event.dataTransfer.setData('text/html', draggedRow.outerHTML);
        draggedRow.style.opacity = '0.5'; // 拖曳時改變透明度
    });

    tableBody.addEventListener('dragover', function (event) {
        event.preventDefault(); // 允許放下
        event.dataTransfer.dropEffect = "move";
    });

    tableBody.addEventListener('drop', function (event) {
        event.preventDefault();
        if (event.target.closest('tr') && draggedRow !== event.target.closest('tr')) {
            const targetRow = event.target.closest('tr'); // 確定 drop 的目標行
            const rows = Array.from(tableBody.querySelectorAll('tr')); // 獲取當前所有行
            const draggedIndex = rows.indexOf(draggedRow);
            const targetIndex = rows.indexOf(targetRow);

            if (draggedIndex > targetIndex) {
                tableBody.insertBefore(draggedRow, targetRow); // 將拖曳行放在目標行之前
            } else {
                tableBody.insertBefore(draggedRow, targetRow.nextSibling); // 放在目標行之後
            }
        }
        draggedRow.style.opacity = '1'; // 拖放結束時恢復透明度
    });

    tableBody.addEventListener('dragend', function () {
        draggedRow = null; // 清空拖曳狀態
    });

    // 標記該 tableBody 已經綁定過
    tableBody.setAttribute('data-drag-bound', 'true');
}


function bindDragEvents() {
    document.querySelectorAll('.table-body').forEach(function (tableBody) {
        enableDragAndDrop(tableBody);
    });
}