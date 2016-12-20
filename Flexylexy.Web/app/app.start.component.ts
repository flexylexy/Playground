import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { HubService } from "./services/hub.service";
import { UserService } from "./services/user.service";

declare var $: any;

@Component({
    selector: 'start',
    templateUrl: './Pages/app.start.component.html'
})
export class StartComponent {
    public name: string;

    constructor(
        private _userService: UserService,
        private router: Router) { }

    public enter() {
        if (this.name) {
            this._userService.name = this.name;
            this.router.navigate(["/lobby"]);
        }
    }

    public onKeyPress(event) {
        if (event.keyCode == 13) {
            this.enter();
        }
    }
}