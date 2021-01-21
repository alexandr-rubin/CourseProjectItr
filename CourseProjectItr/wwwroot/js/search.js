function Search() {
    var input, filter, table, tr, tags, i, txtValue;
    input = document.getElementById("text-to-find");
    filter = input.value.toUpperCase();
    table = document.getElementById("collectionsTable");
    tr = table.getElementsByTagName("tr");
    for (i = 0; i < tr.length; i++) {
        tags = tr[i].getElementsByTagName("tags")[0];
        if (tags) {
            txtValue = tags.textContent || tags.innerText;
            if (txtValue.toUpperCase().indexOf(filter) > -1) {
                tr[i].style.display = "";
            } else {
                tr[i].style.display = "none";
            }
        }
    }
}