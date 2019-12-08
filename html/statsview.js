function handleStats(statsJson) {
  if (document.getElementById('statsTable').style.display == 'none') {
    document.getElementById('statsTable').style.display = 'inline';
    document.getElementById('waitingLabel').style.display = 'none';
  }
  var statsObj = JSON.parse(statsJson);
  document.getElementById('gametime').innerText = statsObj.game_time_formatted;
  document.getElementById('killcount').innerText = statsObj.killed_enemies;
  document.getElementById('enemycount').innerText = statsObj.total_enemies;
  document.getElementById('secretcount').innerText = statsObj.secrets_found;
  document.getElementById('totalsecrets').innerText = statsObj.total_secrets;
}

function handleWaiting() {
  document.getElementById('statsTable').style.display = 'none';
  document.getElementById('waitingLabel').style.display = 'inline';
}
