var toATEList = function (urlATE) {
    $.ajax({
        url: urlATE,
        
        success: function (status) {
                        
            console.log(status);                     
        },
        error: function (status) {
            console.log(status); 
            alert('Error on calling function');
        }
    });
}