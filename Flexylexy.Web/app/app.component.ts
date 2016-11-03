import { Component } from '@angular/core';
import { HubService } from "./services/hub.service";
import { NameService } from "./services/name.service";

@Component({
    selector: 'app',
    templateUrl: './Pages/app.html'
})
export class AppComponent {
    public name: string;

    constructor(
        private _hubService: HubService,
        private _nameService: NameService) {

        this._nameService.nameObservable.subscribe(name => this.name = name);
    }

    public clearPlayers() {
        this._hubService.clearPlayers();
    }
}