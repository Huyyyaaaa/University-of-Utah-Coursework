<template>
    <div class="card card-default">
        <div class="card-header">
            Update Team Information
        </div>

        <div class="card-body">
            <!-- Success Message -->
            <div class="alert alert-success" v-if="form.successful">
                Your team information has been updated!
            </div>

            <form role="form">
                <!-- Address -->
                <div class="form-group row">
                    <label class="col-md-4 col-form-label text-md-right">Address</label>

                    <div class="col-md-6">
                        <input type="text" class="form-control" name="address" v-model="form.address"
                               :class="{'is-invalid': form.errors.has('address')}">

                        <span class="invalid-feedback" v-show="form.errors.has('address')">
                            {{ form.errors.get('address') }}
                        </span>
                    </div>
                </div>
                <!-- City -->
                <div class="form-group row">
                    <label class="col-md-4 col-form-label text-md-right">City</label>

                    <div class="col-md-6">
                        <input type="text" class="form-control" name="city" v-model="form.city"
                               :class="{'is-invalid': form.errors.has('city')}">

                        <span class="invalid-feedback" v-show="form.errors.has('city')">
                            {{ form.errors.get('city') }}
                        </span>
                    </div>
                </div>
                <!-- State -->
                <div class="form-group row">
                    <label class="col-md-4 col-form-label text-md-right">State</label>

                    <div class="col-md-6">
                        <input type="text" class="form-control" name="state" v-model="form.state"
                               :class="{'is-invalid': form.errors.has('state')}">

                        <span class="invalid-feedback" v-show="form.errors.has('state')">
                            {{ form.errors.get('state') }}
                        </span>
                    </div>
                </div>
                <!-- Zip -->
                <div class="form-group row">
                    <label class="col-md-4 col-form-label text-md-right">Zip</label>

                    <div class="col-md-6">
                        <input type="text" class="form-control" name="zip" v-model="form.zip"
                               :class="{'is-invalid': form.errors.has('zip')}">

                        <span class="invalid-feedback" v-show="form.errors.has('zip')">
                            {{ form.errors.get('zip') }}
                        </span>
                    </div>
                </div>
                <!-- Phone -->
                <div class="form-group row">
                    <label class="col-md-4 col-form-label text-md-right">Phone</label>

                    <div class="col-md-6">
                        <input type="text" class="form-control" name="phone" v-model="form.phone"
                               :class="{'is-invalid': form.errors.has('phone')}">

                        <span class="invalid-feedback" v-show="form.errors.has('phone')">
                            {{ form.errors.get('phone') }}
                        </span>
                    </div>
                </div>
                <!-- Email -->
                <div class="form-group row">
                    <label class="col-md-4 col-form-label text-md-right">Team Email</label>

                    <div class="col-md-6">
                        <input type="text" class="form-control" name="email" v-model="form.team_email"
                               :class="{'is-invalid': form.errors.has('team_email')}">

                        <span class="invalid-feedback" v-show="form.errors.has('team_email')">
                            {{ form.errors.get('team_email') }}
                        </span>
                    </div>
                </div>
                <!-- Disclaimer -->
                <div class="form-group row">
                    <label class="col-md-4 col-form-label text-md-right">Disclaimer</label>

                    <div class="col-md-6">
                        <textarea type="text" class="form-control" name="disclaimer" v-model="form.disclaimer"
                                  :class="{'is-invalid': form.errors.has('disclaimer')}"></textarea>

                        <span class="invalid-feedback" v-show="form.errors.has('disclaimer')">
                            {{ form.errors.get('disclaimer') }}
                        </span>
                    </div>
                </div>

                <!-- Update Button -->
                <div class="form-group row mb-0">
                    <div class="offset-md-4 col-md-6">
                        <button type="submit" class="btn btn-primary"
                                @click.prevent="update"
                                :disabled="form.busy">
                            Update
                        </button>
                    </div>
                </div>
            </form>
        </div>
    </div>
</template>

<script>
    import Form from "../../../utils/form";

    export default {
        props: ['user', 'team'],
        data() {
            return {
                form: new Form({
                    address: this.team.address,
                    city: this.team.city,
                    state: this.team.state,
                    zip: this.team.zip,
                    phone: this.team.phone,
                    team_email: this.team.team_email,
                    disclaimer: this.team.disclaimer,
                })
            }
        },
        methods: {
            update() {
                this.form.put('/settings/teams/' + this.team.id + '/information');
            }
        }
    }
</script>
