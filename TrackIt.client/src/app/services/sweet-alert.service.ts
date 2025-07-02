import { Injectable } from '@angular/core';
import Swal, { SweetAlertIcon } from 'sweetalert2';

@Injectable({
  providedIn: 'root'
})
export class SweetAlertService {
  labels: Labels = { ok: 'OK', cancel: 'Cancel' };

  alert(message: string, callback?: (ok: boolean) => void, cssClass?: string) {
    Swal.fire({
      text: message,
      icon: "info",
      customClass: cssClass ? { popup: cssClass } : undefined,
      confirmButtonText: this.labels.ok
    }).then(() => callback?.(true));
    return this;
  }

  confirm(message: string, callback?: (ok: boolean) => void, cssClass?: string) {
    Swal.fire({
      text: message,
      icon: 'question',
      showCancelButton: true,
      confirmButtonText: this.labels.ok,
      cancelButtonText: this.labels.cancel,
      customClass: cssClass ? { popup: cssClass } : undefined
    }).then(result => callback?.(!!result.isConfirmed));
    return this;
  }

  prompt(message: string, callback?: (ok: boolean, val: string) => void, placeholder?: string, cssClass?: string) {
    Swal.fire({
      text: message,
      input: 'text',
      inputPlaceholder: placeholder,
      showCancelButton: true,
      confirmButtonText: this.labels.ok,
      cancelButtonText: this.labels.cancel,
      customClass: cssClass ? { popup: cssClass } : undefined
    }).then(result => callback?.(!!result.isConfirmed, result.value || ''));
    return this;
  }

  success(message: string) {
    Swal.fire({
      text: message,
      icon: 'success',
      timer: 2000,
      showConfirmButton: false
    });
    return this;
  }

  error(message: string) {
    Swal.fire({
      text: message,
      icon: 'error',
      timer: 2000,
      showConfirmButton: false
    });
    return this;
  }

  log(message: string, type: SweetAlertIcon, wait?: number) {
    Swal.fire({
      text: message,
      icon: type,
      timer: wait ?? 2000,
      showConfirmButton: false
    });
    return this;
  }

  extend(type: SweetAlertIcon) {
    return (message: string, wait?: number) => this.log(message, type, wait);
  }

}

// Some helpful labels
export interface Labels {
  ok?: string | undefined;
  cancel?: string | undefined;
}
