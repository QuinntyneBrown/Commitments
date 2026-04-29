import { Injector, runInInjectionContext } from '@angular/core';
import { DashboardBackendService } from '@commitments/dashboard-framework';
import { AuthService } from './auth.service';

describe('AuthService', () => {
  function makeService(post: jest.Mock) {
    const injector = Injector.create({
      providers: [{ provide: DashboardBackendService, useValue: { post } }]
    });
    return runInInjectionContext(injector, () => new AuthService());
  }

  it('posts credentials to api/v1.0/users/token', async () => {
    const post = jest.fn().mockResolvedValue({ accessToken: 'tok', profileId: 'p1' });
    const service = makeService(post);
    const result = await service.token({ username: 'alice', password: 'pw' });
    expect(post).toHaveBeenCalledWith('api/v1.0/users/token', { username: 'alice', password: 'pw' });
    expect(result).toEqual({ accessToken: 'tok', profileId: 'p1' });
  });
});
