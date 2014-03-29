var trackNum = 1;
var startedRec = false;
var widget;

function generateTrackNum(){
	var test = Math.floor((Math.random()*8)+1);
	
	while( test  == trackNum)
		test = Math.floor((Math.random()*8)+1);
		
	trackNum = test;	
		
}

function setUpWidget(){
	var iframeElement   = document.querySelector('iframe');
	widget  = SC.Widget(iframeElement);
}

function recButton(){
	if(!startedRec){
		startedRec = true;
		document.getElementById("recordbutton").innerHTML = "Recording...";
		//figure out 
		widget.seekTo(0);
		widget.play();
		//we then wait for the PLAY_PROGRESS event, knowing that the song is loaded before
		widget.bind(SC.Widget.Events.PLAY_PROGRESS, function(player, data) {
			alert("Playing! " + data);            
		});
	
	}
	else{
	document.getElementById("recordbutton").innerHTML = "Done Recording!";
		//stop recording
		
		//send trackNum using GET
		//send recording using post
	
	
	}
}

