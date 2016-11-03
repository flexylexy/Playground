import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { HubService } from "./services/hub.service";
import { NameService } from "./services/name.service";

declare var $: any;

@Component({
    selector: 'start',
    templateUrl: './Pages/app.start.component.html'
})
export class StartComponent {
    public name: string;

    constructor(
        private _nameService: NameService,
        private _hubService: HubService,
        private router: Router) { }

    public enter() {
        this._nameService.name = this.name;
        this.router.navigate(["/lobby"]);
    }
}