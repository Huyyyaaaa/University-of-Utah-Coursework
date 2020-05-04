<template>
    <div>
        <div class="row">
            <div class="col-6">
                <div class="card container py-4" tour-step="1">
                    <div v-if="!projectEdit">
                        <div class="card-header">
                            <h5>{{ form.name }}</h5>
                            <button
                                class="btn btn-warning"
                                @click="projectEdit = true"
                            >
                                <i class="fas fa-pencil-alt"></i>
                            </button>
                        </div>
                        <div class="container pt-4">
                            <h5 class="sub-header">Description</h5>
                            <div>{{ form.description }}</div>
                        </div>
                        <div class="container pt-4 row">
                            <div class="col">
                                <h5 class="sub-header">Address</h5>
                                <div>{{ form.address }}</div>
                                <div>
                                    {{ form.city }}, {{ form.state }}
                                    {{ form.zip }}
                                </div>
                            </div>
                            <div class="col">
                                <h5 class="sub-header">Client Information</h5>
                                <div>{{ form.contact_name }}</div>
                                <div>
                                    <a :href="'tel:' + form.contact_phone">
                                        {{ form.contact_phone }}
                                    </a>
                                </div>
                                <div>
                                    <a :href="'mailto:' + form.contact_email">
                                        {{ form.contact_email }}
                                    </a>
                                </div>
                            </div>
                        </div>
                        <div class="container pt-4 row">
                            <div class="col">
                                <h5 class="sub-header">Square Feet</h5>
                                <div>{{ form.sq_foot }} sq. ft</div>
                            </div>
                            <div class="col">
                                <h5 class="sub-header">Labor Hours</h5>
                                <div>{{ form.labor_hours }}</div>
                            </div>
                        </div>
                        <div class="container pt-4 row">
                            <div class="col">
                                <h5 class="sub-header">Labor Rate</h5>
                                <div>{{ form.labor_rate | formatPrice }}</div>
                            </div>
                            <div class="col">
                                <h5 class="sub-header">Percentage on Top</h5>
                                <div>{{ form.percentage_on_top }}%</div>
                            </div>
                        </div>
                    </div>
                    <div v-else>
                        <div class="card-header">
                            <h5>Edit Project</h5>
                            <button
                                class="btn btn-success"
                                @click="saveProject"
                            >
                                <i class="fas fa-save"></i>
                            </button>
                        </div>
                        <div class="container">
                            <form>
                                <div class="form-group">
                                    <label for="name">Name</label>
                                    <input
                                        id="name"
                                        type="text"
                                        class="form-control"
                                        v-model="form.name"
                                    />
                                </div>
                                <div class="form-group">
                                    <label for="description">Description</label>
                                    <textarea
                                        id="description"
                                        class="form-control"
                                        v-model="form.description"
                                    ></textarea>
                                </div>
                                <div class="form-group">
                                    <label for="address">Address</label>
                                    <input
                                        type="text"
                                        id="address"
                                        name="address"
                                        class="form-control"
                                        v-model="form.address"
                                    />
                                </div>
                                <div class="form-group">
                                    <label for="city">City</label>
                                    <input
                                        type="text"
                                        id="city"
                                        name="city"
                                        class="form-control"
                                        v-model="form.city"
                                    />
                                </div>
                                <div class="form-group row">
                                    <div class="col">
                                        <label for="state">State</label>
                                        <input
                                            type="text"
                                            id="state"
                                            name="state"
                                            class="form-control"
                                            v-model="form.state"
                                        />
                                    </div>
                                    <div class="col">
                                        <label for="zip">Zip</label>
                                        <input
                                            type="text"
                                            id="zip"
                                            name="zip"
                                            class="form-control"
                                            v-model="form.zip"
                                        />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label for="clientname">Client Name</label>
                                    <input
                                        type="text"
                                        id="clientname"
                                        name="clientname"
                                        class="form-control"
                                        v-model="form.contact_name"
                                    />
                                </div>
                                <div class="form-group row">
                                    <div class="col">
                                        <label for="contactphone"
                                            >Client Phone</label
                                        >
                                        <input
                                            type="text"
                                            id="contactphone"
                                            name="contactphone"
                                            class="form-control"
                                            v-model="form.contact_phone"
                                        />
                                    </div>
                                    <div class="col">
                                        <label for="contactemail"
                                            >Client Email</label
                                        >
                                        <input
                                            type="text"
                                            id="contactemail"
                                            name="contactemail"
                                            class="form-control"
                                            v-model="form.contact_email"
                                        />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label for="sqft"
                                        >Project Square Feet</label
                                    >
                                    <input
                                        type="text"
                                        id="sqft"
                                        name="sqft"
                                        class="form-control"
                                        v-model="form.sq_foot"
                                    />
                                </div>
                                <div class="form-group">
                                    <label for="labor_hours">Labor Hours</label>
                                    <input
                                        type="text"
                                        id="labor_hours"
                                        name="labor_hours"
                                        class="form-control"
                                        v-model="form.labor_hours"
                                    />
                                </div>
                                <div class="form-group">
                                    <label for="labor_rate">Labor Rate</label>
                                    <input
                                        type="text"
                                        id="labor_rate"
                                        name="labor_rate"
                                        class="form-control"
                                        v-model="form.labor_rate"
                                    />
                                </div>
                                <div class="form-group">
                                    <label for="percentage_on_top"
                                        >Percentage on Top</label
                                    >
                                    <input
                                        type="text"
                                        id="percentage_on_top"
                                        name="percentage_on_top"
                                        class="form-control"
                                        v-model="form.percentage_on_top"
                                    />
                                </div>
                            </form>
                            <button
                                class="btn btn-danger"
                                @click="deleteProject"
                            >
                                <i class="fas fa-trash"></i>
                            </button>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-6">
                <div class="card container py-4" tour-step="2">
                    <div class="card-header">
                        <h5>Materials</h5>
                        <button
                            v-if="!materialEdit"
                            class="btn btn-warning"
                            @click="materialEdit = true"
                        >
                            <i class="fas fa-pencil-alt"></i>
                        </button>
                        <button
                            v-else
                            class="btn btn-success"
                            @click="saveProject"
                        >
                            <i class="fas fa-save"></i>
                        </button>
                    </div>
                    <div class="container text-center py-4" v-if="materialEdit">
                        <button
                            @click="$refs.materialModal.getMaterials"
                            class="btn btn-primary"
                            data-toggle="modal"
                            data-target="#materialModal"
                        >
                            <i class="fas fa-plus"></i> Materials
                        </button>
                        <button
                            @click="
                                $refs.projectTemplateModal.getProjectTemplates
                            "
                            class="btn btn-primary"
                            data-toggle="modal"
                            data-target="#projectTemplateModal"
                        >
                            <i class="fas fa-plus"></i> from Template
                        </button>
                    </div>
                    <div class="card-body">
                        <h5 v-if="form.materials <= 0" class="text-center">
                            no materials
                        </h5>
                        <table
                            v-else
                            id="material-project-table"
                            class="table table-hover text-center"
                        >
                            <thead>
                                <tr>
                                    <th>Material</th>
                                    <th>Quantity</th>
                                    <th>Price</th>
                                    <th>Total</th>
                                    <th v-if="materialEdit"></th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr
                                    v-bind:key="index"
                                    v-for="(m, index) in form.materials"
                                >
                                    <td>{{ m.name }}</td>
                                    <td v-if="!materialEdit">
                                        {{ m.quantity }}
                                    </td>
                                    <td v-else>
                                        <input
                                            type="number"
                                            class="form-control"
                                            v-model="
                                                form.materials[index].quantity
                                            "
                                        />
                                    </td>
                                    <td class="price">
                                        {{ m.price | formatPrice }}
                                    </td>
                                    <td class="price">
                                        {{ calculateTotal(m) | formatPrice }}
                                    </td>
                                    <td v-if="materialEdit">
                                        <button
                                            class="btn btn-danger btn-sm"
                                            @click="deleteMaterial(index)"
                                        >
                                            <i class="fas fa-trash"></i>
                                        </button>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
        <br />
        <div class="row">
            <div class="col-6">
                <div class="card container py-4" tour-step="3">
                    <div class="card-header">
                        <h5>Bids</h5>
                        <button
                            class="btn btn-primary"
                            data-toggle="modal"
                            data-target="#bidModal"
                        >
                            New Bid
                        </button>
                        <button
                            class="btn btn-success mx-2"
                            data-toggle="modal"
                            data-target="#autocalculateModal"
                            @click="getAutoFigures"
                        >
                            Autocalculate
                        </button>
                    </div>
                    <div class="card-body">
                        <h5 v-if="project.bids <= 0" class="text-center">
                            No bids created
                        </h5>
                        <table
                            v-else
                            id="bid-project-table"
                            class="table table-hover py-4"
                        >
                            <thead>
                                <tr>
                                    <th>Name</th>
                                    <th>Description</th>
                                    <th>Price</th>
                                    <th></th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr v-for="b in project.bids" v-bind:key="b.id">
                                    <td>{{ b.name }}</td>
                                    <td>{{ b.description | strLimit(25) }}</td>
                                    <td class="price">
                                        {{ b.price | formatPrice }}
                                    </td>
                                    <td>
                                        <a
                                            class="btn btn-primary btn-sm"
                                            :href="`/bid/${b.id}`"
                                        >
                                            <i class="fas fa-eye"></i>
                                        </a>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
            <div class="col-6">
                <div class="card container py-4" tour-step="4">
                    <div class="card-header">
                        <h5>Additional Charges and Discounts</h5>
                        <button
                            class="btn btn-primary"
                            data-toggle="modal"
                            data-target="#adjustmentModal"
                        >
                            Add Adjustment
                        </button>
                    </div>
                    <div class="card-body container">
                        <h5 v-if="adjustments.length <= 0" class="text-center">
                            no adjustments
                        </h5>
                        <table
                            id="adjustmentsTable"
                            v-else
                            class="table table-borderless"
                        >
                            <tbody>
                                <tr
                                    v-for="(adjustment, index) in adjustments"
                                    :key="index"
                                >
                                    <td>{{ adjustment.description }}</td>
                                    <td
                                        v-if="adjustment.amount > 0"
                                        class="price"
                                    >
                                        {{ adjustment.amount | formatPrice }}
                                    </td>
                                    <td v-else class="price-neg">
                                        {{ adjustment.amount | formatPrice }}
                                    </td>
                                    <td>
                                        <button
                                            data-toggle="modal"
                                            data-target="#adjustmentModal"
                                            class="btn btn-warning btn-sm"
                                            @click="
                                                editAdjustment(
                                                    adjustment,
                                                    index
                                                )
                                            "
                                        >
                                            <i class="fas fa-pencil-alt"></i>
                                        </button>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
        <br />
        <!-- Tasks -->
        <div class="row">
            <div class="col-6">
                <div class="card container py-4" tour-step="5">
                    <div class="card-header">
                        <h5>Tasks</h5>
                        <button
                            v-if="!taskSelected"
                            class="btn btn-primary"
                            data-target="#taskcreateModal"
                            data-toggle="modal"
                        >
                            New Task
                        </button>
                    </div>
                    <div v-if="!taskSelected">
                        <div class="card-body">
                            <h5
                                v-if="projectTasks.length <= 0"
                                class="text-center"
                            >
                                No tasks for project
                            </h5>
                            <table v-else class="table table-hover py-4">
                                <thead>
                                    <tr>
                                        <th></th>
                                        <th>Description</th>
                                        <th>Due</th>
                                        <th></th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr
                                        v-for="(task, idx) in projectTasks"
                                        :key="task.id"
                                    >
                                        <td>
                                            <i
                                                v-if="task.completed"
                                                @click="markComplete(task, idx)"
                                                class="fas fa-check-circle"
                                            ></i>
                                            <i
                                                v-else
                                                @click="markComplete(task, idx)"
                                                class="far fa-circle"
                                            ></i>
                                        </td>
                                        <td class="desc">
                                            {{
                                                task.description | strLimit(50)
                                            }}
                                        </td>
                                        <td class="task-date">
                                            {{ formatDate(task.due_at) }}
                                        </td>
                                        <td>
                                            <button
                                                class="btn btn-warning btn-sm"
                                                @click="taskClicked(task)"
                                            >
                                                <i
                                                    class="fas fa-pencil-alt"
                                                ></i>
                                            </button>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                    <div v-else>
                        <label for="taskDesc">Description</label>
                        <textarea
                            id="taskDesc"
                            class="form-control"
                            v-model="taskForm.description"
                        >
                        </textarea>
                        <br />
                        <label>Due at</label>
                        <datetime
                            input-id="due-date"
                            input-class="form-control"
                            v-model="taskForm.due_at"
                            type="datetime"
                            use12-hour
                        ></datetime>
                        <br />
                        <div>
                            <button class="btn btn-success" @click="saveTask">
                                <i class="fas fa-save"></i>
                            </button>
                            <button class="btn btn-danger" @click="deleteTask">
                                <i class="fas fa-trash"></i>
                            </button>
                        </div>
                        <br>
                        <div id="created" class="col-6">
                            <div>
                                <span>{{ selectedTask.created_at }}</span>
                                <span>{{ selectedTask.created_by.name }}</span>
                            </div>
                            <div class="img-container">
                                <img
                                    class="created-img"
                                    :src="selectedTask.created_by.photo_url"
                                />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-6">
                <div class="card container py-4">
                    <div class="card-header">
                        <h5>Project Location</h5>
                    </div>
                    <div class="card-body">
                        <iframe
                            width="100%"
                            height="400"
                            frameborder="0"
                            style="border:2px solid gray"
                            :src="address"
                            allowfullscreen
                        ></iframe>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-10 mt-4 offset-1">
            <div class="card" tour-step="6">
                <div class="card-header">
                    <h5>Project Photos</h5>
                </div>
                <div class="card-body">
                    <file-pond
                        name="photo"
                        ref="pond"
                        label-idle="Drop photos here or anywhere on the page..."
                        allow-multiple="true"
                        accepted-file-types="image/jpeg, image/png"
                        :server="server"
                        labelFileTypeNotAllowed="Invalid File Type"
                        fileValidateTypeLabelExpectedTypes="Only JPEG and PNG File Types Are Allowed."
                        :onprocessfile="handleOnProcessFile"
                        dropOnPage="true"
                        dropOnElement="false"
                    />
                </div>
                <div v-if="photos.length > 0" class="card-footer">
                    <div v-if="!clicked" class="scrollmenu">
                        <a v-for="photo in photos" v-bind:key="photo">
                            <img
                                :src="photo"
                                alt
                                width="200"
                                @click="imageClicked(photo)"
                                style="max-height:250px"
                            />
                        </a>
                    </div>
                    <div v-else align="center">
                        <a>
                            <img
                                :src="photo"
                                alt
                                @click="imageClicked(photo)"
                                width="700"
                                style="border:2px solid black"
                            />
                        </a>
                    </div>
                </div>
            </div>
        </div>
        <!-- modal for adjustments -->
        <div
            id="adjustmentModal"
            class="modal"
            tabindex="-1"
            role="dialog"
            ref="adjustmentmodal"
        >
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header card-header">
                        <h5 v-if="!adjustmentEdit">New Adjustment</h5>
                        <h5 v-else>Edit Adjustment</h5>
                        <button
                            type="button"
                            class="close"
                            data-dismiss="modal"
                            aria-label="Close"
                        >
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <label for="bidDesc">Description</label>
                            <textarea
                                id="bidDesc"
                                class="form-control"
                                v-model="adjustmentForm.description"
                            ></textarea>
                        </div>
                        <div class="form-group">
                            <label>Amount</label>
                            <money
                                id="adjustmentPrice"
                                class="form-control"
                                v-model="adjustmentForm.amount"
                            ></money>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button
                            v-if="!adjustmentEdit"
                            class="btn btn-primary"
                            data-dismiss="modal"
                            @click="addAdjustment"
                        >
                            Save
                        </button>
                        <button
                            style="margin-right:40%;"
                            data-dismiss="modal"
                            v-if="adjustmentEdit"
                            class="btn btn-danger"
                            @click="deleteAdjustment"
                        >
                            <i class="fas fa-trash"></i>
                        </button>
                        <button
                            v-if="adjustmentEdit"
                            class="btn btn-primary"
                            data-dismiss="modal"
                            @click="saveEditedAdjustment"
                        >
                            Save
                        </button>
                        <button
                            type="button"
                            class="btn btn-secondary"
                            data-dismiss="modal"
                        >
                            Cancel
                        </button>
                    </div>
                </div>
            </div>
        </div>
        <!-- modal for autocalculate -->
        <div id="autocalculateModal" class="modal" tabindex="-1" role="dialog">
            <div class="modal-dialog modal-lg" role="document">
                <div class="modal-content">
                    <div class="modal-header card-header">
                        <h5>Auto Calculate Bid</h5>
                        <button
                            type="button"
                            class="close"
                            data-dismiss="modal"
                            aria-label="Close"
                        >
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body" v-if="autocalculateForm">
                        <table class="table table-hover">
                            <colgroup>
                                <col width="33%" />
                                <col width="33%" />
                                <col width="33%" />
                            </colgroup>
                            <thead>
                                <tr>
                                    <th>Material</th>
                                    <th>Qty</th>
                                    <th>Total</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr
                                    v-for="(m, idx) in form.materials"
                                    v-bind:key="idx"
                                >
                                    <td>{{ m.name }}</td>
                                    <td>
                                        {{ m.quantity }}
                                        <i class="fas fa-arrow-right m-2"></i>
                                        {{ autocalculateForm[idx].quantity }}
                                    </td>
                                    <td>
                                        {{ calculateTotal(m) | formatPrice }}
                                        <i class="fas fa-arrow-right m-2"></i>
                                        {{
                                            autocalculateForm[idx].total
                                                | formatPrice
                                        }}
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div class="modal-footer">
                        <button
                            type="button"
                            class="btn btn-primary"
                            data-dismiss="modal"
                            @click="autoCalculate"
                        >
                            Save
                        </button>
                        <button
                            type="button"
                            class="btn btn-secondary"
                            data-dismiss="modal"
                        >
                            Cancel
                        </button>
                    </div>
                </div>
            </div>
        </div>
        <material-search-modal ref="materialModal"></material-search-modal>
        <project-template-modal
            ref="projectTemplateModal"
        ></project-template-modal>
        <bid-create-modal
            ref="bidModal"
            :data="form"
            :adjustments="adjustments"
        ></bid-create-modal>

        <task-create-modal :project="project.id" ref="taskcreateModal"></task-create-modal>
        <v-tour
            name="myTour"
            :steps="tourSteps"
            :options="tourOptions"
            :callbacks="tourCallbacks"
        ></v-tour>
    </div>
</template>

<script>
import taskCreateModal from "../shared/task-create-modal";
import MaterialSearchModal from "../shared/material-search-modal";
import ProjectTemplateModal from "../shared/project-template-modal";
import BidCreateModal from "../shared/bid-create-modal";
import vueFilePond from "vue-filepond";
import "filepond/dist/filepond.min.css";
import "filepond-plugin-image-preview/dist/filepond-plugin-image-preview.min.css";
import FilePondPluginFileValidateType from "filepond-plugin-file-validate-type";
import FilePondPluginImagePreview from "filepond-plugin-image-preview";
import FilePondPluginImageTransform from "filepond-plugin-image-transform";
import FilePondPluginImageExifOrientation from "filepond-plugin-image-exif-orientation";

let token = document.head.querySelector('meta[name="csrf-token"]');
const FilePond = vueFilePond(
    FilePondPluginFileValidateType,
    FilePondPluginImagePreview,
    FilePondPluginImageTransform,
    FilePondPluginImageExifOrientation
);

export default {
    name: "project-show",
    components: { BidCreateModal, ProjectTemplateModal, MaterialSearchModal },
    props: ["project"],
    data() {
        return {
            photos: [],
            server: {
                process: {
                    url: "/api/project/" + this.project.id + "/photo",
                    method: "POST",
                    headers: {
                        "X-CSRF-TOKEN": token.content
                    }
                },
                revert: (response, load, error) => {
                    let photo = JSON.parse(response).photo;
                    axios
                        .delete(
                            route("api.project.photo.delete", {
                                project: photo.project_id,
                                token: photo.token
                            })
                        )
                        .catch(err => error(err.response.message));
                    load();
                }
            },
            address: null,
            clicked: false,
            photo: null,
            projectEdit: false,
            materialEdit: false,
            tablesLoaded: false,
            adjustmentEdit: false,
            projectTasks: null,
            form: {
                name: this.project.name,
                description: this.project.description,
                materials: this.project.materials,
                project_id: this.project.id,
                address: this.project.address,
                city: this.project.city,
                state: this.project.state,
                zip: this.project.zip,
                contact_name: this.project.contact_name,
                contact_phone: this.project.contact_phone,
                contact_email: this.project.contact_email,
                sq_foot: this.project.sq_foot,
                labor_hours: this.project.labor_hours,
                labor_rate: this.project.labor_rate,
                percentage_on_top: this.project.percentage_on_top
            },
            adjustments: this.project.extra_charges,
            adjustmentForm: null,
            adjustmentIndex: null,
            autocalculateForm: null,
            taskForm: {
                project_id: this.project.id,
                description: null,
                due_at: null,
                completed: null
            },
            taskSelected: false,
            newTask: false,
            selectedTask: null,
            tourOptions: {
                useKeyboardNavigation: false,
                labels: {
                    buttonSkip: "Skip",
                    buttonPrevious: "Previous",
                    buttonNext: "Next",
                    buttonStop: "Ok"
                }
            },
            tourCallbacks: {
                onSkip: this.stopTour
            },
            tourSteps: [
                {
                    target: '[tour-step="1"]',
                    header: {
                        title: "Congrats!"
                    },
                    content:
                        "You have created a Project! You can see and edit project details here",
                    params: {
                        placement: "top"
                    }
                },
                {
                    target: '[tour-step="2"]',
                    content: "You can see and edit your project materials here",
                    params: {
                        placement: "top"
                    }
                },
                {
                    target: '[tour-step="3"]',
                    content:
                        'You can see your bids here. To make a new bid click "New Bid" or "Autocalculate" to have your bid auto-calculated',
                    params: {
                        placement: "top"
                    }
                },
                {
                    target: '[tour-step="4"]',
                    content:
                        "You can add additional charges and discounts here",
                    params: {
                        placement: "top"
                    }
                },
                {
                    target: '[tour-step="5"]',
                    content: "Tasks associated with your project will be here",
                    params: {
                        placement: "top"
                    }
                },
                {
                    target: '[tour-step="6"]',
                    content: "You can upload photos of your project here",
                    params: {
                        placement: "top"
                    }
                },
                {
                    target: '[side-step="4"]',
                    content: "Last but not least, let's head over to Tasks",
                    params: {
                        placement: "right"
                    }
                }
            ]
        };
    },
    created() {
        this.initAdjustmentForm();
        Bus.$on("materialsAdded", data => {
            this.tablesLoaded = false
            this.addToMaterials(data);
        });
        Bus.$on("projectTemplatesAdded", data => {
            this.tablesLoaded = false
            this.addToMaterials(data);
        });
        Bus.$on("taskCreated", data => {
            this.tablesLoaded = false
            this.projectTasks.push(data);
        });
    },
    mounted() {
        this.fetchPhotos();
        this.projectTasks = this.project.tasks;

        this.address =
            "https://www.google.com/maps/embed/v1/place?key=AIzaSyCkrQLUu184Vye0ohxkt9hSlW0ZNDk8GQA&q=" +
            (
                this.form.address +
                "+" +
                this.form.city +
                "+" +
                this.form.state +
                "+" +
                this.form.zip.substring(0, 5)
            ).replace(/\s+/g, "+");

        $(this.$refs.adjustmentmodal).on("hide.bs.modal", () => {
            this.initAdjustmentForm();
        });
    },
    updated() {
        if (!this.checkTour()) {
            this.$tours["myTour"].start();
        }
            if(!this.tablesLoaded){
            $("#material-project-table").dtable({ pageLimit: 5 });
            $("#bid-project-table").dtable({ pageLimit: 5, filter: false });
            $("#adjustmentsTable").dtable({
                pageLimit: 5,
                header: false,
                filter: false
            });
                this.tablesLoaded = true
            }
         
    },
    methods: {
        markComplete(task, index) {
            if (task.completed) {
                Vue.set(task, "completed", 0);
            } else {
                Vue.set(task, "completed", 1);
            }

            axios
                .put(`/api/task/${task.id}`, task)
                .then(res => {
                    this.project.tasks[
                        this.project.tasks.findIndex(x => x.id === task.id)
                    ] = res.data.task;
                    this.$parent.$refs.mytoast.s("Task successfully updated!");
                })
                .catch(function(err) {
                    $.each(err.response.data.errors, function(index, error) {
                        this.$parent.$refs.mytoast.e(error[0]);
                    });
                });
        },
        formatDate(date) {
            return moment(date).format("M/D/YY[\n]h:mm a");
        },
        initAdjustmentForm() {
            this.adjustmentForm = {
                description: null,
                amount: 0.0
            };
        },
        handleOnProcessFile(error, resourceFile) {
            let self = this;
            setTimeout(() => {
                self.$refs.pond.removeFile(resourceFile.id);
                let photo = JSON.parse(resourceFile.serverId).photo;
                self.photos.push(photo.path);
            }, 2000);
        },
        fetchPhotos() {
            let self = this;
            axios
                .get("/api/project/" + this.project.id + "/photo")
                .then(data => {
                    self.photos = data.data;
                });
        },
        materialAddedAlready(material) {
            return (
                this.form.materials.filter(
                    _material => material.id === _material.id
                ).length > 0
            );
        },
        addToMaterials(data) {
            let self = this;
            $.each(data, function(index, material) {
                if (!self.materialAddedAlready(material)) {
                    self.form.materials.push(material);
                }
            });
        },
        saveProject() {
            this.projectEdit = false;
            this.materialEdit = false;

            let self = this;
            axios
                .put(`/api/project/${this.project.id}`, this.form)
                .then(res => {
                    self.$parent.$refs.mytoast.s(res.data.message);
                })
                .catch(function(err) {
                    $.each(err.response.data.errors, function(index, error) {
                        self.$parent.$refs.mytoast.e(error[0]);
                    });
                });
        },
        calculateTotal(m) {
            return m.price * m.quantity;
        },
        deleteMaterial(idx) {
            this.tablesLoaded = false;
            this.form.materials.splice(idx, 1);
        },
        deleteProject() {
            axios.delete(`/api/project/${this.project.id}`).then(data => {
                if (data.data.status === "success") {
                    window.location = "/project";
                } else {
                    Swal.fire({
                        title: "Error",
                        text: data.data.message,
                        type: "error",
                        confirmButtonText: "OK"
                    });
                }
            });
        },
        addAdjustment() {
            let self = this;
            axios
                .post(
                    "/api/project/" + this.project.id + "/extraCharge/create",
                    this.adjustmentForm
                )
                .then(response => {
                    self.$parent.$refs.mytoast.s(response.data.message);
                    self.adjustments.push(response.data.extraCharge);
                    self.initAdjustmentForm();
                }).catch(function(err) {
                    $.each(err.response.data.errors, function(index, error) {
                        self.$parent.$refs.mytoast.e(error[0]);
                    });
                });
        },
        editAdjustment(adjustment, index) {
            this.adjustmentEdit = true;
            this.adjustmentIndex = index;
            this.adjustmentForm.id = adjustment.id;
            this.adjustmentForm.description = adjustment.description;
            this.adjustmentForm.amount = adjustment.amount;
        },
        saveEditedAdjustment() {
            let self = this;
            axios
                .put(
                    "/api/project/" +
                        this.project.id +
                        "/extraCharge/" +
                        this.adjustmentForm.id,
                    this.adjustmentForm
                )
                .then(response => {
                    self.$parent.$refs.mytoast.s(response.data.message);
                    self.adjustments[self.adjustmentIndex] =
                        response.data.extraCharge;
                    self.initAdjustmentForm();
                    this.adjustmentEdit = false;
                    $(this.$refs.adjustmentmodal).modal("hide");
                }).catch(function(err) {
                    $.each(err.response.data.errors, function(index, error) {
                        self.$parent.$refs.mytoast.e(error[0]);
                    });
                });
        },
        deleteAdjustment() {
            let self = this;
            axios
                .delete(
                    "/api/project/" +
                        this.project.id +
                        "/extraCharge/" +
                        this.adjustmentForm.id,
                    this.adjustmentForm
                )
                .then(response => {
                    self.$parent.$refs.mytoast.s(response.data.message);
                    self.adjustments.splice(self.adjustmentIndex, 1);
                    self.initAdjustmentForm();
                    this.adjustmentEdit = false;
                    this.tablesLoaded = false;
                    $(this.$refs.adjustmentmodal).modal("hide");
                }).catch(function(err) {
                    $.each(err.response.data.errors, function(index, error) {
                        self.$parent.$refs.mytoast.e(error[0]);
                    });
                });
        },
        getAutoFigures() {
            this.autocalculateForm = this.project.materials;
            var squarefeet = this.form.sq_foot;
            $.each(this.autocalculateForm, function(index, mat) {
                var newQuantity = Math.ceil(squarefeet / mat.sq_foot_coverage);
                var newPrice = mat.price * newQuantity;
                mat.quantity = newQuantity;
                mat.total = newPrice;
            });
        },
        autoCalculate() {
            if (this.autocalculateForm != null) {
                //this.form.materials = this.autocalculateForm;
                //Vue.set(this.form, materials, this.autocalculateForm)
                for (let i = 0; i < this.autocalculateForm.length; i++) {
                    Vue.set(this.form.materials, i, this.autocalculateForm[i]);
                }
            }
            this.saveProject();
        },
        imageClicked(photo) {
            let self = this;
            self.photo = photo;
            if (self.clicked) {
                self.clicked = false;
                self.photo = null;
            } else {
                self.photo = photo;
                self.clicked = true;
            }
        },
        taskClicked(selectedTask) {
            let self = this;
            self.taskSelected = true;
            self.selectedTask = selectedTask;
            let d = new Date(self.selectedTask.due_at);
            self.taskForm.completed = selectedTask.completed;
            self.taskForm.description = selectedTask.description;
            self.taskForm.due_at = d.toISOString();
        },
        saveTask() {
            let date = moment(this.taskForm.due_at).format(
                "MMMM DD, YYYY, HH:mm"
            );
            this.taskForm.due_at = date;

            axios
                .put(`/api/task/${this.selectedTask.id}`, this.taskForm)
                .then(res => {
                    if (res.data.status === "success") {
                        this.projectTasks[
                            this.projectTasks.findIndex(
                                x => x.id === this.selectedTask.id
                            )
                        ] = res.data.task;
                    }
                    this.$parent.$refs.mytoast.s("Task successfully updated!");
                    this.exitTask();
                })
                .catch(err => {
                    if (err.message == "Request failed with status code 403") {
                        this.$parent.$refs.mytoast.e(
                            "You can only edit your own Tasks"
                        );
                    } else {
                        this.$parent.$refs.mytoast.e("Unable to Save Changes");
                    }
                    this.exitTask();
                });
        },
        deleteTask() {
            let self = this;
            axios
                .delete(`/api/task/${this.selectedTask.id}`)
                .then(data => {
                    if (data.data.status === "success") {
                        self.projectTasks.splice(
                            self.projectTasks.findIndex(
                                x => x.id === self.selectedTask.id
                            ),
                            1
                        );
                        this.$parent.$refs.mytoast.s(
                            "Task successfully deleted!"
                        );
                    }
                })
                .catch(function(err) {
                    self.$parent.$refs.mytoast.e(err.response.data.message);
                })
                .finally(function() {
                    self.exitTask();
                });
        },

        exitTask() {
            let self = this;
            self.newTask = false;
            self.taskSelected = false;
            self.taskDueAtDate = null;
            self.selectedTask = null;
            self.taskDueAtTime = null;
            self.taskForm.description = null;
            self.taskForm.completed = null;
            self.taskForm.due_at = null;
        }
    }
};
</script>

<style scoped>
div.scrollmenu {
    background-color: #333;
    overflow: auto;
    white-space: nowrap;
}

div.scrollmenu a {
    display: inline-block;
    color: white;
    text-align: center;
    padding: 14px;
    text-decoration: none;
}

div.scrollmenu a:hover {
    background-color: #777;
}
.fa-circle {
    color: #b3b3b3;
}
.fa-check-circle {
    color: #3cd660;
}
i:hover {
    cursor: pointer;
}
.task-date {
    white-space: nowrap;
    font-size: 12px;
}
#created div span {
  display: block;
  font-size: 11px;
  color: #b3b3b3;
}
#created div {
  display: inline-block;
}
.created-img{
  width: 35px;
  height: 35px;
  border-radius: 100px;
}
</style>
