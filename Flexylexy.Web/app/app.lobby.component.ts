import { Component } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { HubService } from "./services/hub.service";
import { NameService } from "./services/name.service";

declare var $: any;

@Component({
    selector: 'lobby',
    templateUrl: './Pages/app.lobby.component.html'
})
export class LobbyComponent {
    public players;
    private subscriptions = [];

    constructor(
        private router: Router,
        private _hubService: HubService,
        private _nameService: NameService) { }

    ngOnInit() {
        this.subscribeToEvents();
        
        this._hubService.getPlayers().done((players) => {
            this.players = players.filter(x => (x.ConnectionId != this._hubService.getConnectionId()));
        });
    }

    private subscribeToEvents() {
        var self = this;

        this.subscriptions[0] = this._hubService.playersUpdated.subscribe((data) => {
            self.players = data.players.filter(x => (x.ConnectionId != self._hubService.getConnectionId()));
        });

        this.subscriptions[1] = this._hubService.challenged.subscribe((data) => {
            var accepted = confirm("You have been challenged to a game by " + data.opponentName);

            if (accepted) {
                this._hubService.acceptChallenge(data.opponentConnectionId);
                this.router.navigate(['/tictactoe', data.opponentConnectionId, false]);
            } else {
                this._hubService.declineChallenge(data.opponentConnectionId);
            }
        });

        this.subscriptions[2] = this._hubService.challengeAccepted.subscribe((opponentConnectionId) => {
            alert("Challenge accepted.");
            this.router.navigate(['/tictactoe', opponentConnectionId, true]);
        });

        this.subscriptions[3] = this._hubService.challengeDeclined.subscribe(() => alert("Challenge declined."));
    }

    public challenge(player) {
        this._hubService.challenge(this._nameService.name, player.ConnectionId);
    }

    ngOnDestroy() {
        this.subscriptions.forEach(x => x.unsubscribe());
    }
}