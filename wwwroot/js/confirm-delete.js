var item;
var itemId;

$(document).on('click', '.deleteItem', function () {
    item = $(this);
    itemId = item.attr('data-id');

    $('#deleteModal').modal('show');
    $('#deleteModal #message').text('Confirm to delete?');

});


$('#deleteModal #confirm').on('click', function () {

    $.ajax({
        type: "DELETE",
        url: "../GoodsReceipt/DeleteItem?itemId=" + itemId,
        success: function (data) {
            if (data == 'success') {
                window.location.reload();
            }
        },
        error: function (error) {
            console.log(error);
        },
    });





});




