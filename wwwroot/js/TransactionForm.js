function initTransactionForm(urlCategories) {
    $("#OperationTypeId").change(async function () {
        const value = $(this).val();

        const response = await fetch(urlCategories,{
            method: 'POST',
            body: value,
            headers:{
                'Content-Type': 'application/json'
            }
        });

        const json = await response.json();
        const options = json.map(category => `<option value="${category.value}">${category.text}</option>`);
        $("#CategoryId").html(options);
    })
}