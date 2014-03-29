var trackNum = 1;

function() generateTrackNum{
	var test = Math.floor((Math.random()*8)+1);
	
	while( test  == trackNum)
		test = Math.floor((Math.random()*8)+1);
		
	trackNum = test;	
		
	alert(trackNum);
}