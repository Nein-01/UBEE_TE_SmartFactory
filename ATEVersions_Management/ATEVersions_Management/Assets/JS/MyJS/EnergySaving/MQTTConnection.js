
var client;
function connectToServer() {
	var brokerUrl = document.getElementById("broker-url").value;
	var brokerUser = document.getElementById("broker-user").value;
	var brokerPass = document.getElementById("broker-pass").value;

	client = mqtt.connect(brokerUrl, { username: brokerUser, password: brokerPass });

	client.on("connect", function () {
		//console.log("Connected to broker");
		document.getElementById("connect-status").innerHTML = "Connected"
	});

	client.on("message", function (topic, message) {
		//console.log("Received message on topic: " + topic);
		//console.log("Message: " + message.toString());
		var messagesDiv = document.getElementById("messages");
		var messageElem = document.createElement("p");
		messageElem.innerHTML = "<strong>" + topic + "</strong>: " + message.toString();
		messagesDiv.appendChild(messageElem);
	});
}

function subscribeToTopic() {
	var subscribeTopic = document.getElementById("subscribe-topic").value;
	client.subscribe(subscribeTopic);
}

function sendMessage() {
	var publishTopic = document.getElementById("publish-topic").value;
	var message = document.getElementById("message").value;
	client.publish(publishTopic, message);
}