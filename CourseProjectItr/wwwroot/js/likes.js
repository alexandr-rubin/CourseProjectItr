function likeclick(id, lc) {
    lc++;
    $.ajax({
        type: 'POST',
        url: '/Collections/Like',
        data: { "id": id },
        success: function (content) {
            $('#like_' + id).val('Like ' + content);
        }
    })
};