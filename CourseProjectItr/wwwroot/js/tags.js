function AddTheme() {
    var input = document.querySelector('input[id="input-custom-dropdown"]'),
        tagify = new Tagify(input, {
            whitelist: ["Books", "Images", "Music", "Videos", "Films", "Other"],
            maxTags: 1,
            dropdown: {
                maxItems: 20,
                classname: "tags-look",
                enabled: 0,
                closeOnSelect: false
            }
        })
};