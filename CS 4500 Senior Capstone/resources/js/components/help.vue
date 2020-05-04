<template>
    <div>
        <div class="card">
            <div class="card-header">
                <center>
                    <strong>
                        <h1 style="color: black;">
                            <img src="../../../public/img/logo/Regular/logo-presentation.png" height="100"/>
                            <br>
                            Help and Support:
                        </h1>
                    </strong>
                </center>
            </div>
            <div class="card">
                <div class="card-body">
                    <!-- Material Drop Down -->
                    <button type="button" class="collapsible" @click="clicked">Materials</button>
                    <div class="content">
                        <button type="button" class="collapsible" @click="clicked">Create</button>
                        <div class="content">
                            <div class="card" >
                                <div class="card" >

                                    <div class="card-body" style="color: black;">
                                        <h4>1. Individual Materials:</h4>
                                        <ol>
                                            <li>Navigate to the material page from the side bar menu</li>
                                            <li>Click the 'CREATE' button in the top right side of page</li>
                                            <li>Fill in the new material form with:</li>
                                            <ul>
                                                <li>Material Name</li>
                                                <li>Material Description</li>
                                                <li>Material Cost Per Unit</li>
                                                <li>Square Feet Covered Per Unit</li>
                                            </ul>
                                            <li>Click the 'Create' button at the buttom of the form or click 'Cancel' to leave without creating
                                                a new material</li>
                                        </ol>
                                    </div>
                                </div>
                                <div class="card" >

                                    <div class="card-body" style="color: black;">
                                        <h4>2. Multiple Materials:</h4>
                                        <ol>
                                            <li>Create a CSV file in the following format for the desired materials to create:</li>
                                            <table id="fileFormatExample" class="table">
                                                <thead>
                                                <tr>
                                                    <th>Name</th>
                                                    <th>Description</th>
                                                    <th>Price</th>
                                                    <th>Sq Foot Coverage</th>
                                                    <th></th>
                                                </tr>
                                                </thead>

                                                <tr>
                                                    <th>Material1</th>
                                                    <th>This is the first Material</th>
                                                    <th>1.00</th>
                                                    <th>5</th>
                                                </tr>
                                                <tr>
                                                    <th>Material2</th>
                                                    <th>This is the second Material</th>
                                                    <th>2.22</th>
                                                    <th>10</th>
                                                </tr>
                                                <tr>
                                                    <th>Material3</th>
                                                    <th>This is the third Material</th>
                                                    <th>0.30</th>
                                                    <th>10</th>
                                                </tr>
                                                <tbody>
                                                </tbody>
                                            </table>
                                            <li>Navigate to the material page from the side bar menu</li>
                                            <li>Click the 'CHOOSE A FILE' button in the bottom left side of page</li>
                                            <li>Navigate and select the .csv file you would like to upload</li>
                                            <li>Click the 'LOAD' button located next to the 'CHOOSE A FILE' button and your materials will be uploaded from the file</li>
                                        </ol>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <button type="button" class="collapsible" @click="clicked">Update</button>
                        <div class="content">
                            <div class="card" >

                                <div class="card-body" style="color: black;">
                                    <ol>
                                        <li>Navigate to the material page from the side bar menu</li>
                                        <li>
                                            Navigate to the material you would like to update by using the search bar in the top right or
                                            navigate through the pages
                                        </li>
                                        <li>Click the eye button next to the desired material</li>
                                        <li>In the material page click the yellow pen and paper button to the side of the material name</li>
                                        <li>Update the desired fields and click the green save icon</li>
                                    </ol>
                                </div>
                            </div>
                        </div>
                        <button type="button" class="collapsible" @click="clicked">Delete</button>
                        <div class="content">
                            <div class="card" >

                                <div class="card-body" style="color: black;">
                                    <p>Note: to delete a material it cannot be used in project template</p>
                                    <ol>
                                        <li>Navigate to the material page from the side bar menu</li>
                                        <li>
                                            Navigate to the material you would like to delete by using the search bar in the top right or
                                            navigate through the pages
                                        </li>
                                        <li>Click the eye button next to the desired material</li>
                                        <li>In the material page click the yellow pen and paper button to the side of the material name</li>
                                        <li>Click the red trashcan button at the bottom of the page</li>
                                    </ol>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- Project Template Drop Down -->

                    <button type="button" class="collapsible" @click="clicked">Project Templates</button>
                    <div class="content">
                        <button type="button" class="collapsible" @click="clicked">Create</button>
                        <div class="content">
                            <div class="card" >

                                <div class="card-body" style="color: black;">

                                    <ol>
                                        <li>Navigate to the templates page from the side bar menu</li>
                                        <li>Click the 'CREATE' button in the top right side of page</li>
                                        <li>Fill in the new template form with:</li>
                                        <ul>
                                            <li>Template Name</li>
                                            <li>Template Description</li>
                                            <li>Click the '+Materials' button and add the materials you would like for the template</li>
                                        </ul>
                                        <li>
                                            Click the 'Create' button at the bottom of the form or click 'Cancel' to leave without creating the template
                                        </li>
                                    </ol>
                                </div>
                            </div>
                        </div>
                        <button type="button" class="collapsible" @click="clicked">Update</button>
                        <div class="content">
                            <div class="card" >

                                <div class="card-body" style="color: black;">

                                    <ol>
                                        <li>Navigate to the templates page from the side bar menu</li>
                                        <li>
                                            Navigate to the template you would like to update by using the search bar in the top right or
                                            navigate through the pages
                                        </li>
                                        <li>Click the eye button next to the desired material</li>
                                        <li>In the template page click the yellow pen and paper button to the side of the template creator</li>
                                        <li>Update the desired fields and click the green save icon</li>
                                    </ol>
                                </div>
                            </div>
                        </div>
                        <button type="button" class="collapsible" @click="clicked">Delete</button>
                        <div class="content">
                            <div class="card" >

                                <div class="card-body" style="color: black;">

                                    <ol>
                                        <li>Navigate to the templates page from the side bar menu</li>
                                        <li>
                                            Navigate to the template you would like to delete by using the search bar in the top right or
                                            navigate through the pages
                                        </li>
                                        <li>Click the eye button next to the desired material</li>
                                        <li>In the template page click the yellow pen and paper button to the side of the template creator</li>
                                        <li>Click the red trashcan button at the bottom of the page</li>
                                    </ol>
                                </div>
                            </div>
                        </div>
                    </div>
                    <!-- Project and Bids Drop Down -->

                    <button type="button" class="collapsible" @click="clicked">Projects and Bids</button>
                    <div class="content">
                        <button type="button" class="collapsible" @click="clicked">Create</button>
                        <div class="content">
                            <button type="button" class="collapsible" @click="clicked">Project</button>
                            <div class="content">
                                <div class="card" >

                                    <div class="card-body" style="color: black;">

                                        <ol>
                                            <li>Navigate to the projects page from the side bar menu</li>
                                            <li>Click the 'CREATE' button in the top right side of page</li>
                                            <li>Fill in the new project form with:</li>
                                            <ul>
                                                <li>Project Name and Description</li>
                                                <li>Project Location</li>
                                                <li>Client Information</li>
                                                <li>Project Overhead Costs</li>
                                            </ul>
                                            <li>Add materials:</li>
                                            <ul>
                                                <li>Use the materials button to select individual materials</li>
                                                <li>Use the templates button to select materials from a project template</li>
                                            </ul>
                                            <li>
                                                Click the 'Create' button at the bottom of the form or click 'Cancel' to leave without creating the project
                                            </li>
                                        </ol>
                                    </div>
                                </div>
                            </div>
                            <button type="button" class="collapsible" @click="clicked">Bids</button>
                            <div class="content">
                                <div class="card" >

                                    <div class="card-body" style="color: black;">
                                        <ol>
                                            <li>Navigate to the project you would like to make a bid for</li>
                                            <li>In the 'BIDS' section click the 'New Bid'</li>
                                            <li>Fill in the Bid Name and Bid Description</li>
                                            <li>Click the 'Create' button or click 'Cancel' to leave without creating the bid</li>
                                        </ol>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <button type="button" class="collapsible" @click="clicked">Update</button>
                        <div class="content">
                            <div class="card" >

                                <div class="card-body" style="color: black;">

                                    <p>Note: you cannot update bids</p>
                                    <ol>
                                        <li>Navigate to the project you would like to update</li>
                                        <li>To update project name/description, client information, and overhead costs:</li>
                                        <ol>
                                            <li>Click the yellow pen and paper button in the upper left panel</li>
                                            <li>Update form information</li>
                                        </ol>
                                        <li>To update materials:</li>
                                        <ol>
                                            <li>Click the yellow pen and paper button in the upper right(Materials) panel</li>
                                            <li>Add or remove desired materials</li>
                                        </ol>
                                        <li>Click the green save icon</li>
                                    </ol>
                                </div>
                            </div>
                        </div>
                        <button type="button" class="collapsible" @click="clicked">Delete</button>
                        <div class="content">
                            <div class="card" >

                                <div class="card-body" style="color: black;">

                                    <p>Note: Bids cannot be deleted and projects cannot be deleted if they have bids</p>
                                    <ol>
                                        <li>Navigate to the project you would like to delete</li>
                                        <li>Click the yellow pen and paper button in the upper left panel</li>
                                        <li>In the template page click the red trashcan at the bottom of the form</li>
                                    </ol>
                                </div>
                            </div>
                        </div>
                    </div>
                    <!-- Tasks Drop Down -->

                    <button type="button" class="collapsible" @click="clicked">Tasks</button>
                    <div class="content">
                        <button type="button" class="collapsible" @click="clicked">Create</button>
                        <div class="content">
                            <div class="card" >

                                <div class="card-body" style="color: black;">
                                    <h4>1. Inside a project (This task will be associated to the project it is created in):</h4>
                                    <ol>
                                        <li>Navigate to the project you would like to create a task for</li>
                                        <li>Navigate to the 'TASKS' form and click the 'New Task' button</li>
                                        <li>Fill in the form with new task information</li>
                                        <li>Click the green save icon to save the task or the red trash can to cancel the task creation</li>
                                    </ol>
                                </div>
                            </div>
                            <div class="card" >

                                <div class="card-body" style="color: black;">

                                    <h4>2. Task Page:</h4>
                                    <ol>
                                        <li>Navigate to the task page on the side bar menu</li>
                                        <li>Click the 'Create' button on the upper right side of the pane</li>
                                        <li>Fill in the form with new task information</li>
                                        <li>Click the green save icon to save the task or cancel  to exit without creating the task</li>
                                    </ol>
                                </div>
                            </div>
                        </div>
                        <button type="button" class="collapsible" @click="clicked">Update</button>
                        <div class="content">
                            <div class="card" >

                                <div class="card-body" style="color: black;">
                                    <ol>
                                        <li>Navigate to the task page from the side bar menu</li>
                                        <li>Navigate to the task you would like to update and select it by clicking on it</li>
                                        <li>The task will open in a form on the right side of the page</li>
                                        <li>Update the task information in the form as desired</li>
                                        <li>Click the green save button at the top right of the form to save the changes</li>
                                    </ol>
                                </div>
                            </div>
                        </div>
                        <button type="button" class="collapsible" @click="clicked">Delete</button>
                        <div class="content">
                            <div class="card" >

                                <div class="card-body" style="color: black;">
                                    <ol>
                                        <li>Navigate to the task page from the side bar menu</li>
                                        <li>Navigate to the task you would like to delete and select it by clicking on it</li>
                                        <li>The task will open in a form on the right side of the page</li>
                                        <li>Click the red trash can button at the top right of the form to delete the task</li>
                                    </ol>
                                </div>
                            </div>
                        </div>
                    </div>
                    <!-- User Drop Down -->
                    <button type="button" class="collapsible" @click="clicked">User Information and Settings</button>
                    <div class="content">
                        <button type="button" class="collapsible" @click="clicked">Update Profile Picture</button>
                        <div class="content">
                            <div class="card" >
                                <div class="card-body" style="color: black;">
                                    <ol>
                                        <li>Click on the user name and profile picture in the upper right hand corner</li>
                                        <li>From the drop down select 'Your Settings'</li>
                                        <li>Select the 'Profile' tab</li>
                                        <li>Navigate to the file you would like to use for you new profile picture</li>
                                        <li>Click 'Open' to set the selected file as your profile picture</li>
                                    </ol>
                                </div>
                            </div>
                        </div>
                        <button type="button" class="collapsible" @click="clicked">Update Name and Email</button>
                        <div class="content">
                            <div class="card" >
                                <div class="card-body" style="color: black;">
                                    <ol>
                                        <li>Click on the user name and profile picture in the upper right hand corner</li>
                                        <li>From the drop down select 'Your Settings'</li>
                                        <li>Select the 'Profile' tab</li>
                                        <li>In the form on the right side update your name and or password</li>
                                        <li>Click the 'Update' button to save the changes</li>
                                    </ol>
                                </div>
                            </div>
                        </div>
                        <button type="button" class="collapsible" @click="clicked">Update Password</button>
                        <div class="content">
                            <div class="card" >
                                <div class="card-body" style="color: black;">
                                    <ol>
                                        <li>Click on the user name and profile picture in the upper right hand corner</li>
                                        <li>From the drop down select 'Your Settings'</li>
                                        <li>Select the 'Security' tab</li>
                                        <li>Fill in the form with your current pass word and what you would like your new password to be</li>
                                        <li>Click the 'Update' button to save the changes</li>
                                    </ol>
                                </div>
                            </div>
                        </div>
                    </div>
                    <!-- Teams Drop Down-->
                    <button type="button" class="collapsible" @click="clicked">Teams Management</button>
                    <div class="content">
                        <button type="button" class="collapsible" @click="clicked">Invite Someone to Join a Team</button>
                        <div class="content">
                            <div class="card" >
                                <div class="card-body" style="color: black;">
                                    <ol>
                                        <li>Click on the user name and profile picture in the upper right hand corner</li>
                                        <li>From the drop down select 'Your Settings'</li>
                                        <li>Select the 'Teams' tab</li>
                                        <li>Click the cog button next to the team you would like to invite a member to join</li>
                                        <li>In the form at the top of the page enter the users email and the position level you would like to make them</li>
                                        <li>Click 'Send Invitation' to send them an email to join the team</li>
                                    </ol>
                                </div>
                            </div>
                        </div>
                        <button type="button" class="collapsible" @click="clicked">Leave a Team</button>
                        <div class="content">
                            <div class="card" >
                                <div class="card-body" style="color: black;">
                                    <ol>
                                        <li>Click on the user name and profile picture in the upper right hand corner</li>
                                        <li>From the drop down select 'Your Settings'</li>
                                        <li>Select the 'Teams' tab</li>
                                        <li>Click the yellow arrow button next to the team you would like to leave</li>
                                        <li>In the pop-up click 'Yes, Leave' to leave the team</li>
                                    </ol>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

</template>


<script>
    export default {
        data() {
            return {}
        },

        mounted() {

        },

        methods: {
            clicked() {
                var coll = Array.from(document.getElementsByClassName("collapsible"));
                var i;

                for (i = 0; i < coll.length; i++) {
                    coll[i].addEventListener("click", function() {
                        this.classList.toggle("active");
                        var content = this.nextElementSibling;
                        if (content.style.display === "block") {
                            content.style.display = "none";
                        } else {
                            content.style.display = "block";
                        }
                    });
                }
            },

        }
    }
</script>
<style>
    .collapsible {
        background-color: white;
        color: black;
        cursor: pointer;
        padding: 18px;
        width: 100%;
        border-width:5px;
        border-style:solid;
        text-align: left;
        outline: none;
        font-size: 15px;


    }
    .collapsible:hover {
        background-color: #555;
        color: white;
    }
    .content {
        padding: 0 18px;
        display: none;
        overflow: hidden;
        background-color: #f1f1f1;
    }
</style>
