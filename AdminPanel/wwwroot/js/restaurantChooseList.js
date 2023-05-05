
function setupPagination(url, container) {
    $(".page-link").on("click", function () {
        getPage(url)
            .then(result => container.html(result));
    });

    $(".page-link").first().click();
}