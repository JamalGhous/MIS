<div class="container">
<div class="well"  style="background-color: white;">
<fieldset>
<legend>Find Patient Record</legend>

<div id="DivInputSearch" class="form-group has-success has-feedback">
  <input type="text" class="form-control" id="InputPatient" aria-describedby="inputError2Status" placeholder="Search By ID or Name">
  
  <span
  	id="InputPatientError"
  	style="display: none; cursor: pointer;"
  	class="glyphicon glyphicon-remove form-control-feedback"
  	aria-hidden="true"
 	>
	</span>

</div>

<br>
<?php include("PatientTable.php"); ?>
</fieldset>
</div>
</div>

<script type="text/javascript">
$(document).ready(function()
{
	$("#InputPatient").keyup(function(event)
	{
		if($("#InputPatient").val().trim())
		{
			$("#InputPatientError").show();
    	}
    	else
    	{
    		$("#InputPatientError").hide();
    	}
	});
});


</script>