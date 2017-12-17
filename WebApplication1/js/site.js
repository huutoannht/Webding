// Write your JavaScript code.
function deleteRecord(id,action) {
    swal({
        title: "Bạn muốn xóa dòng này?",
        text: "Dòng này sẽ được xóa khỏi hệ thống!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#f8b32d",
        confirmButtonText: "Yes, delete it!",
        closeOnConfirm: false
    }, function () {
        $.ajax({
            type: "POST",
            url: "/" + action + "/Delete",
            data: { id: id },
            success: function () {
                //swal("Deleted!", "Xóa thành công.", "success");
                window.location.reload(true);
            },
        });

    });
    return false;
};