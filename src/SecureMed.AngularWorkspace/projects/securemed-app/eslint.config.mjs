// @ts-check
import tseslint from 'typescript-eslint';
import rootConfig from '../../eslint.config.mjs';

export default tseslint.config(...rootConfig, {
	files: ['**/*.ts'],
	rules: {
		'@angular-eslint/directive-selector': [
			'error',
			{
				type: 'attribute',
				prefix: 'securemed',
				style: 'camelCase',
			},
		],
		'@angular-eslint/component-selector': [
			'error',
			{
				type: 'element',
				prefix: 'securemed',
				style: 'kebab-case',
			},
		],
		'@angular-eslint/component-class-suffix': 'off',
		'@angular-eslint/component-directive-suffix': 'off',
	},
});
