import { Component } from '@angular/core';
import { FormGroup, AbstractControl } from '@angular/forms';
@Component({
  template: ''
})
export abstract class BaseFormComponent {

  form!: FormGroup;

  constructor() { }

  // return error message depending on validation error type
  // custom error message can be provided with format {errorkey: custommessage}
  getErrors(
    control: AbstractControl,
    displayName: string,
    customMessages: { [key: string]: string } | null = null
  ): string[] {
    var errors: string[] = [];
    Object.keys(control.errors || {}).forEach((key) => {
      switch (key) {
        case 'required':
          errors.push(`${displayName} ${customMessages?.[key] ?? "is required."}`);
          break;
        case 'pattern':
          errors.push(`${displayName} ${customMessages?.[key] ?? "contains invalid characters."}`);
          break;
        case 'maxlength':
          errors.push(`${displayName} ${customMessages?.[key] ?? "exceed its maximum size."}`)
          break;
        case 'minlength':
          errors.push(`${displayName} ${customMessages?.[key] ?? "requires more characters"}`)
          break;
        default:
          errors.push(`${displayName} is invalid.`);
          break;
      }
    });
    return errors;
  }
  
}
