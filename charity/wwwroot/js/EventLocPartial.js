//from: https://www.w3schools.com/html/html5_draganddrop.asp

let draggedRow;

function drag(event) {
    draggedRow = event.target.closest('tr'); // 記住被拖動的行
    event.dataTransfer.effectAllowed = 'move';
}

function allowDrop(event) {
    event.preventDefault(); // 必須阻止默認行為以允許drop
    event.dataTransfer.dropEffect = 'move'; // 指示這是移動操作
}

function drop(event) {
    event.preventDefault(); // 阻止默認行為

    let targetRow = event.target.closest('tr'); // 找到被放置的目標行
    if (!draggedRow || !targetRow || draggedRow === targetRow) {
        return; // 避免拖放無效情況
    }

    let tbody = document.getElementById('table-body');

    // 檢查 `draggedRow` 是在 `targetRow` 上方還是下方
    let draggedRowIndex = Array.from(tbody.children).indexOf(draggedRow);
    let targetRowIndex = Array.from(tbody.children).indexOf(targetRow);

    if (draggedRowIndex < targetRowIndex) {
        // 如果是往下拖動，插入到目標行的下方
        tbody.insertBefore(draggedRow, targetRow.nextSibling);
    } else {
        // 往上拖動時，插入到目標行的上方
        tbody.insertBefore(draggedRow, targetRow);
    }
}

// 綁定事件到所有表格行
//document.querySelectorAll('tr[draggable="true"]').forEach(function (row) {
//    /*row.addEventListener('dragstart', drag);*/
//    row.addEventListener('dragover', allowDrop);
//    row.addEventListener('drop', drop);
//});

function bindDragEvents() {
    $('#table-body').on('dragstart', 'tr[draggable="true"]', drag);
    $('#table-body').on('dragover', 'tr[draggable="true"]', allowDrop);
    $('#table-body').on('drop', 'tr[draggable="true"]', drop);
}