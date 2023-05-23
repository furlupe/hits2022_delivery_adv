$(".numbers").on("click", function () {
    $(".numbers").removeClass("active");
    $(this).addClass("active");
})

function getPage(url) {
    return $.ajax({
        url: url,
        type: 'GET',
        async: true
    });
}