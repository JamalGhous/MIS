<?php
class template
{ 
    //public $aMemberVar = 'aMemberVar Member Variable'; 
    //public $aFuncName = 'aMemberFunc'; 
    
    
    function text_input($id, $placeholder, $label, $value, $help, $type, $for)
    {
    	$for=$for."input";
        //Text input
        echo  "	<div class='form-group'>
        			<label class=' control-label' for=$for>$label</label>
            		<div class=''>
            			<input
              				id=$id
              				name=$id
              				placeholder=$placeholder
              				class='form-control input-md'
              				type=$type
              			>";

        				if(!empty($help) && isset($help))
        				{
        					echo "<span class='help-block'>$help</span>";
        				}
        			
        	echo  "</div>
        		</div>";
    } 
}
?>