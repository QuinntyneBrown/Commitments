import { Injector, runInInjectionContext } from '@angular/core';
import { DashboardBackendService } from '@commitments/dashboard-framework';
import { ProfileService } from './profile.service';

describe('ProfileService', () => {
  function makeService(get: jest.Mock) {
    const injector = Injector.create({
      providers: [{ provide: DashboardBackendService, useValue: { get } }]
    });
    return runInInjectionContext(injector, () => new ProfileService());
  }

  it('fetches profile list from api/v1.0/profiles', async () => {
    const profiles = [{ profileId: 1, name: 'Alice' }];
    const get = jest.fn().mockResolvedValue({ profiles });
    const service = makeService(get);
    const result = await service.list();
    expect(get).toHaveBeenCalledWith('api/v1.0/profiles');
    expect(result).toEqual({ profiles });
  });

  it('fetches current profile from api/v1.0/profiles/current', async () => {
    const profile = { profileId: 1, name: 'Alice' };
    const get = jest.fn().mockResolvedValue({ profile });
    const service = makeService(get);
    const result = await service.current();
    expect(get).toHaveBeenCalledWith('api/v1.0/profiles/current');
    expect(result).toEqual({ profile });
  });
});
