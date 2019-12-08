String.prototype.toDurationString = function() {
  var sec_num = parseInt(this, 10);
  var hours = Math.floor(sec_num / 3600);
  var minutes = Math.floor((sec_num - hours * 3600) / 60);
  var seconds = sec_num - hours * 3600 - minutes * 60;

  if (hours < 10) {
    hours = '0' + hours;
  }
  if (minutes < 10) {
    minutes = '0' + minutes;
  }
  if (seconds < 10) {
    seconds = '0' + seconds;
  }
  var durationString = minutes + ':' + seconds;
  if (hours > 0) {
    durationString = hours + ':' + durationString;
  }
  return durationString;
};

function handleStats(statsJson) {
  if (document.getElementById('statsTable').style.display == 'none') {
    document.getElementById('statsTable').style.display = 'inline';
    document.getElementById('waitingLabel').style.display = 'none';
  }
  var statsObj = JSON.parse(statsJson);
  var durationSeconds = (statsObj.game_tic_count / 35).toString();
  var durationString = durationSeconds.toDurationString();
  document.getElementById('gametime').innerText = durationString;
  document.getElementById('killcount').innerText = statsObj.killed_enemies;
  document.getElementById('enemycount').innerText = statsObj.killed_enemies + statsObj.remaining_enemies;
  document.getElementById('secretcount').innerText = statsObj.secrets_found;
  document.getElementById('totalsecrets').innerText = statsObj.total_secrets;
}

function handleWaiting() {
  document.getElementById('statsTable').style.display = 'none';
  document.getElementById('waitingLabel').style.display = 'inline';
}
