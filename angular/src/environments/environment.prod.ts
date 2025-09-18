import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

export const environment = {
  production: true,
  application: {
    baseUrl,
    name: 'EduOutcome',
  },
  apis: {
    default: {
      url: 'https://localhost:44354',
      rootNamespace: 'PSA.EduOutcome',
    },
  },
  remoteEnv: {
    url: '/getEnvConfig',
    mergeStrategy: 'deepmerge'
  }
} as Environment;
