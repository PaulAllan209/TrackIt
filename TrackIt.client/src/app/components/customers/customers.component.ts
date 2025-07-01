import { Component } from '@angular/core';

import { TodoDemoComponent } from '../controls/todo-demo.component';

@Component({
    selector: 'app-customers',
    templateUrl: './customers.component.html',
    styleUrl: './customers.component.scss',
    imports: [TodoDemoComponent]
})
export class CustomersComponent {

}
