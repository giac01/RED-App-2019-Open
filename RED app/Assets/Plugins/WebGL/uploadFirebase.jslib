mergeInto(LibraryManager.library, {
    AddDataToFirebase: function (inputJSONRaw){
        
        //get database reference 
        var database = firebase.database(); 
        
        database.ref(window.uid)
        var inputJSON = Pointer_stringify(inputJSONRaw); 
        var obj = JSON.parse(inputJSON);
        firebase.database().ref('users/' + window.uid).set(obj, 
        function(error) {
                if (error) {
                    console.log('error');
                } else {
                    console.log('success');
                }
            }
        );
        console.log('Upload to Firebase Complete <3')
    
    }
}, {
    Hello: function () { 
            window.alert("Hello, world!"); 
        }
    }
);


