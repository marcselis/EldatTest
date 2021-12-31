import { Component, OnInit } from '@angular/core';
import { DataService, Lamp } from '../data.service';
import { Observable } from 'rxjs';


@Component({
  selector: 'app-lamps',
  templateUrl: './lamps.component.html',
  styleUrls: ['./lamps.component.scss']
})
export class LampsComponent implements OnInit {

  lamps: Lamp[];
  error: any;

  constructor(private data: DataService) { }

  ngOnInit() {
    this.data.getLamps().subscribe(
      lampData => this.lamps = lampData, error=>this.error=error      
    )
}

}


