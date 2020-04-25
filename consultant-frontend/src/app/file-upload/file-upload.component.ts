import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { FileShareService } from '../_services/file-share.service';

@Component({
  selector: 'app-file-upload',
  templateUrl: './file-upload.component.html',
  styleUrls: ['./file-upload.component.css']
})
export class FileUploadComponent implements OnInit {
  @ViewChild('fileSelect', { static: false }) fileSelect: ElementRef< HTMLInputElement >;

  constructor(private fileShare: FileShareService) { }

  ngOnInit(): void {
  }

  onChange(files: FileList): void {
    this.fileShare.uploadFileList(files).then( () => {
      this.fileSelect.nativeElement.value = null;
    })
      .then(r => this.fileShare.listUserFilesAsString().then(m => console.log(m)))
      .catch(e => console.log("File control error in file-upload: " + e));
  }
}
