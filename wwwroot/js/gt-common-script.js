var batchNo;
var batchList = [];
initializeBatchList();

document.addEventListener("DOMContentLoaded", function () {
    document.getElementById("BatchNumber").focus();
    batchNo = $("#BatchNumber").val();
});

$('#BatchNumber').on('keyup',function () {
    batchNo = $(this).val();
    if (batchNo.length >= 10) {
        scanBatchTransaction(batchNo);
    }
});


function scanBatchTransaction(batchNo) {
    if (isExistBatch(batchNo)) {
        return;
    }

    $.ajax({
        type: "get",
        url: "../../GoodsTransfer/LoadDataFromPls",
        data: {
            batchNo,
            trxId,
            movementType
        },
        success: function (data) {
            var flag = 1;
            $("#batchTable .batchNo").each(function (i) {
                if ($(this).val() == data.batchNumber) {
                    flag = 0;
                }
            });

            $("input[name='BatchNumber']").val('');

            if (flag) {
                var newRow = $(`<tr data-id=${data.batchNumber}>`);
                var cols = '';
                cols += `<td class="text-danger"><i class="fa-regular fa-trash-can js-delete" data-id="${data.batchNumber}"></i></td>`;
                cols += `<td class="rowNo"></td>`;
                cols += `<td>${data.batchNumber}</td>`;
                cols += `<td>${data.qty}</td>`;
                cols += `<td>${data.materialNumber}</td>`;
                cols += `<input type="hidden" class="qty" name="qty[]" value="${data.qty}" />`;

                newRow.append(cols);
                $("#batchTable tbody").prepend(newRow);

                calculateTotal();
            }
        },
        error: function (error) {
            $("input[name='BatchNumber']").val('');
            let htmlText = `<p class="text-dark">${error.responseText}</p>`;
            Swal.fire({
                icon: 'error',
                title: `<span class="text-danger">Invalid data!</span>`,
                html: htmlText,
                confirmButtonColor: '#1877f2',
            });
        }
    });
    
}

function initializeBatchList() {
    $.get("../../GoodsTransfer/InitializeBatchList/" + trxId, function (response) {
        response.forEach(function (data) {
            var newRow = $(`<tr data-id=${data.batchNumber}>`);
            var cols = '';
            cols += `<td class="text-danger"><i class="fa-regular fa-trash-can js-delete" data-id="${data.batchNumber}"></i></td>`;
            cols += `<td class="rowNo"></td>`;
            cols += `<td>${data.batchNumber}</td>`;
            cols += `<td>${data.qty}</td>`;
            cols += `<td>${data.materialNumber}</td>`;
            cols += `<input type="hidden" class="qty" name="qty[]" value="${data.qty}" />`;

            newRow.append(cols);
            $("#batchTable tbody").prepend(newRow);
        });

        calculateTotal();
    });
}


function isExistBatch(batchNo) {

    if (batchList.includes(batchNo)) {
        // $('.existBatchNo').text(batchNo + "alredy exists!");
        $("input[name='BatchNumber']").val('');

        return true;
    } else {
        batchList.push(batchNo);
        return false;
    }
}


// calculate total qty
function calculateTotal() {
    var totalQty = 0;
    var rowNo = 0;

    $("#batchTable .rowNo").each(function () {
        rowNo++;
        $(this).html(rowNo);
    });
    $('#totalRecord').text(rowNo);

    $(".qty").each(function () {
        if ($(this).val() == '') {
            totalQty += 0;
        } else {
            totalQty += parseFloat($(this).val());
        }
    });
    $("th.totalQty").text(totalQty.toFixed(3));
}


// remove table row item
$(document).on('click', '.js-delete', function () {
    batchNo = $(this).attr('data-id');
    let button = $(this);
    Swal.fire({
        title: 'Are you sure?',
        text: "You want to remove this batch " + batchNo,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            removeRowItem(batchNo, button);
        }
    })
});


// delete item
function removeRowItem(batchNo, button) {
    $.ajax({
        type: "DELETE",
        url: "../../GoodsTransfer/Delete/" + batchNo + "/" + trxId,
        success: function (data) {
            button.parents('tr').remove();
            calculateTotal();

            let index = batchList.indexOf(batchNo);
            if (index > -1) {
                batchList.splice(index, 1);
            }
        },
        error: function (error) {
            console.log(error);
        }
    });
}


// send to SAP
$('#btnSendToSAP').on('click', function () {
    batchList = [];
    $("#batchTable tbody tr").each(function () {
        batchList.push($(this).attr('data-id'));
    });
    $.ajax({
        type: "POST",
        url: "../../GoodsTransfer/SendToSAP",
        data: {
            batchList,
            trxId
        },
        success: function (response) {
            responseMessage(response);
        },
        error: function (error) {
            console.log(error);
        }
    });
});

function responseMessage(response) {
    $("#responseTable tbody").children().remove();
    response.forEach(function (data) {
        var newRow = $(`<tr>`);
        var cols = '';
        cols += `<td>${data.batchNo}</td>`;

        if (data.materialDocNo.length > 0) {
            cols += `<td>${data.materialDocNo}</td>`;
            cols += `<td>S</td>`;
            cols += `<td>N/A</td>`;
            cols += `<td>N/A</td>`;
        } else {
            cols += `<td class="text-danger">${data.zReturns[0].errorMessage}</td>`;
            cols += `<td class="text-danger">${data.zReturns[0].statusType}</td>`;
            cols += `<td class="text-danger">${data.zReturns[0].messageNo}</td>`;
            cols += `<td class="text-danger">${data.zReturns[0].mmMessageId}</td>`;
        }

        newRow.append(cols);
        $("#responseTable tbody").prepend(newRow);
    });

    $('#SAPResponse').removeClass('d-none');
}