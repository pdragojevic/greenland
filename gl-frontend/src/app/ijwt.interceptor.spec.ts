import { TestBed } from '@angular/core/testing';

import { IjwtInterceptor } from './ijwt.interceptor';

describe('IjwtInterceptor', () => {
  beforeEach(() => TestBed.configureTestingModule({
    providers: [
      IjwtInterceptor
      ]
  }));

  it('should be created', () => {
    const interceptor: IjwtInterceptor = TestBed.inject(IjwtInterceptor);
    expect(interceptor).toBeTruthy();
  });
});
