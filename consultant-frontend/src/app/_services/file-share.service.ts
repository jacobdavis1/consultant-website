import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { ShareServiceClient, ShareDirectoryClient, ShareClient, AnonymousCredential, DirectoryGetPropertiesResponse } from "@azure/storage-file-share";
import { AuthService } from './auth.service';
import { AuthApiService, Auth0User } from './auth-api.service';
import { HttpResponse } from '@angular/common/http';


@Injectable({
  providedIn: 'root'
})
export class FileShareService {

  private anonymousCredential: AnonymousCredential;
  private serviceClient: ShareServiceClient;
  private shareClient: ShareClient;
  private directoryClient: ShareDirectoryClient;

  constructor(private auth: AuthService, private authApi: AuthApiService) {
    this.anonymousCredential = new AnonymousCredential();
    this.serviceClient = new ShareServiceClient(`https://${environment.azure_accountName}.file.core.windows.net?${environment.azure_accountSas}`, this.anonymousCredential);
    this.shareClient = this.serviceClient.getShareClient(environment.azure_shareName);
  }

  // Gets the directory name for the current user, which is their email currently
  async getUserDirectoryName(): Promise<string> {
    let user = await this.auth.getUser$().toPromise();
    return user.email;
  }

  // Check if a directory exists
  async directoryExists(directoryClient: ShareDirectoryClient): Promise<boolean> {
    let propertiesResponse = await this.directoryClient.getProperties().catch(e => console.log("Directory may not exist. Error getting directory properties: " + e));
    // If the properties call returns null, the directory does not exist.
    if (propertiesResponse == null) {
      return false
    } else {
      return true;
    }
  }

  // Initialize the ShareDirectoryClient and create the folder if needed
  async initializeDirectory(directoryName: string): Promise<void> {
    this.directoryClient = this.shareClient.getDirectoryClient(`${directoryName}`);
    if (!(await this.directoryExists(this.directoryClient))) {
      await this.directoryClient.create().catch(e => console.log("Directory create error: " + e));
    }
  }

  // Get a string with a list of files and directories in the current user directory
  async listUserFilesAsString(): Promise<string> {

    let finalList: string = "Files: \n";

    await this.initializeDirectory(await this.getUserDirectoryName()); 
    let iter = await this.directoryClient.listFilesAndDirectories();
    let entity = await iter.next();

    while (!entity.done) {
      if (entity.value.kind === "directory") {
        finalList += `${this.directoryClient.name} - directory\t: ${entity.value.name}\n`;
      } else {
        finalList += `${this.directoryClient.name} - file\t: ${entity.value.name}\n`;
      }

      entity = await iter.next();
    }

    return finalList;
  }

  // Takes a file list and attempts to upload it to the user's folder in the site share
  async uploadFileList(files: FileList): Promise<void> {
    
    let directoryName = await this.getUserDirectoryName();

    for (var i = 0; i < files.length; ++i) {
      await this.initializeDirectory(directoryName);
      console.log("In directory: " + this.directoryClient.name + " using " + this.directoryClient.shareName);

      // Get the file handler
      let fileClient = this.directoryClient.getFileClient(files[i].name);
      console.log("File share object successfully created for " + files[i].name + "!");

      // Initialize the file. Overwrites if already exists
      await fileClient.create(files[i].size).catch(e => console.log("File create error: " + e));
      console.log(files[i].name + " successfully created!");

      // Upload data to initialized file location
      await fileClient.uploadRange(await files[i].arrayBuffer(), 0, files[i].size)
        .then(res => console.log("Successfully uploaded " + files[i].size + " bytes of data. Response: " + res._response.status))
        .catch(e => console.log("File upload error:" + e));
    }
  }
}
